using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Common.Helpers
{
    public static class TableSchemeUtil
    {
        /// <summary>
        /// Table Scheme construction from parsed logical proposition and constructed node tree for the proposition
        /// </summary>
        /// <param name="parsedString">  a list of tokens; the parsed logical proposition</param>
        /// <param name="propositionTree"> the tree constructed from the logical proposition</param>
        public static TableScheme ConstructTableScheme(List<Token> parsedString)
        {
            // Construct Tree
            Node propositionTree = TreeConstructor.ConstructTree(parsedString);

            // Init Table Scheme
            TableScheme tableScheme = new TableScheme();

            // Construct header row
            List<Token> predicateTokens = parsedString.Where(x => x.IsPredicate).GroupBy(x => x.Char).Select(x => x.First()).ToList();
            if (predicateTokens.Count == 0) return tableScheme;

            predicateTokens = predicateTokens.OrderBy(x => x.ToString()).ToList();
            foreach (Token predicate in predicateTokens)
            {
                tableScheme.Predicates.Add(predicate.ToString());
                tableScheme.TableHeaders.Add(predicate.ToString());
            }
            // Add result cell at the end of header row
            tableScheme.TableHeaders.Add("Result");

            // Data construction row by row
            int binaryLength = predicateTokens.Count;
            double numberOfRows = Math.Pow(2, predicateTokens.Count);

            for (int rowNum = 0; rowNum < numberOfRows; rowNum++)
            {
                DataRow row = new DataRow
                {
                    RowNum = rowNum // Give rownumber to each row
                };

                List<Tuple<Token, bool>> subsitutionTuples = new List<Tuple<Token, bool>>();

                // Transform the rownumber to Binary, ex. rownum - 1, with 3 predicates, results in 001
                // The transformed rownumber is casted to array of chars
                char[] rowNumIntoBinary = GeneralHelper.ToBin(row.RowNum, binaryLength).ToCharArray();

                for (var j = 0; j < rowNumIntoBinary.Length; j++)
                // for each binary number
                {
                    // Asign value to Row values and create a tuple for replacing tokens
                    Token predicateToken = predicateTokens[j];
                    bool predicateValue = rowNumIntoBinary[j] == '1';
                    Tuple<Token, bool> replacingToken = new Tuple<Token, bool>(predicateToken, predicateValue);
                    subsitutionTuples.Add(replacingToken);

                    row.Values.Add(predicateValue ? "1" : "0");
                }
                // calculate the result of the row
                bool resultVal = propositionTree.CalculateRowResult(subsitutionTuples);
                row.Result = resultVal;

                // add row to the table
                tableScheme.DataRows.Add(row);
            }

            return tableScheme;
        }


        /// <summary>
        /// Table Scheme construction and then simplification
        /// </summary>
        /// <param name="parsedString">  a list of tokens; the parsed logical proposition</param>        
        public static TableScheme SimplifyTableScheme(List<Token> parsedString)
        {
            TableScheme tblScheme = ConstructTableScheme(parsedString);
            bool completed = false;

            // Loop until there is no further simplification
            while (completed == false)
            {
                completed = true; // Assume completion (if simplification is applied, completed becomes false)
                List<DataRow> simplifiedDataRows = new List<DataRow>(); // Collection of newly created simplified Rows
                int rowCount = 0;
                List<int> alreadySimplifiedRowsIndeces = new List<int>();

                for (int i = 0; i < tblScheme.NrOfDataRows; i++)
                // Loop through each row
                {
                    DataRow currentRow = tblScheme.DataRows[i];
                    bool isRowSimplified = false;

                    for (int j = i + 1; j < tblScheme.NrOfDataRows; j++)
                    // Find possible simplifications, by looping through all rows below the currentRow
                    {
                        DataRow rowToCompare = tblScheme.DataRows[j];

                        // If the row have different result, then don't compare
                        if (currentRow.Result != rowToCompare.Result) continue;

                        // Count the number of differences in between the predicate columns
                        List<int> indecesForChange = new List<int>();
                        for (int columnNr = 0; columnNr < tblScheme.NrOfPredicates; columnNr++)
                        {
                            if (currentRow.Values[columnNr] != rowToCompare.Values[columnNr])
                            {
                                indecesForChange.Add(columnNr);
                            }
                        }

                        // If there are more than one column which differs, then continue iterations
                        if (indecesForChange.Count != 1) continue;

                        // Simplify
                        DataRow newRow = new DataRow
                        {
                            Result = currentRow.Result,
                            RowNum = rowCount,
                            Values = currentRow.Values.ToList()
                        };
                        newRow.Values[indecesForChange[0]] = "*";
                        if (simplifiedDataRows.Any(x => x.Values.SequenceEqual(newRow.Values)) == false)
                        {
                            simplifiedDataRows.Add(newRow);
                        }

                        // Those two rows participated in simplification, hence not visible any further; Store their indexes
                        alreadySimplifiedRowsIndeces.Add(i);
                        alreadySimplifiedRowsIndeces.Add(j);

                        isRowSimplified = true;
                        completed = false;
                    }

                    if (isRowSimplified == false && alreadySimplifiedRowsIndeces.Contains(i) == false)
                    // No simplification was done to this row, hence it remains visible
                    {
                        simplifiedDataRows.Add(currentRow);
                    }
                }
                tblScheme.DataRows = simplifiedDataRows;
            }
            return tblScheme;
        }


       

        /// <summary>
        /// Convert a table scheme to a DNF expression
        /// </summary>
        /// <param name="tblScheme"> Table scheme which is converted to DNF</param>
        /// <returns> Tuple, with Item1 being the DNF string and Item2 is the Hashcode resulting from the DNF </returns>
        public static Tuple<string, string> GetDnf(TableScheme tblScheme)
        {
            Tuple<string, string> result = null;
            List<DataRow> truthRows = tblScheme.DataRows.Where(x => x.Result).ToList();
            List<DataRow> falseRows = tblScheme.DataRows.Where(x => x.Result == false).ToList();

            // Check for Tautology or Contradiction
            if (truthRows.Count == 0) return new Tuple<string, string>("0", tblScheme.TableSchemeToHashCode());
            if (falseRows.Count == 0) return new Tuple<string, string>("1", tblScheme.TableSchemeToHashCode());


            List<List<string>> predicateEq = new List<List<string>>();
            foreach (DataRow truthRow in truthRows)
            {
                List<string> transformedPredicates = new List<string>();
                for (var i = 0; i < truthRow.Values.Count; i++)
                {

                    string predicateValue = truthRow.Values[i];
                    string predicate = tblScheme.Predicates[i];


                    switch (predicateValue)
                    {
                        case "1":
                            transformedPredicates.Add(predicate);
                            break;
                        case "0":
                            transformedPredicates.Add($"~({predicate})");
                            break;
                        case "*":
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                }
                predicateEq.Add(transformedPredicates);

            }
            List<string> propositions = new List<string>();

            // Conjunct all predicate evaluations
            foreach (var listOfPred in predicateEq)
            {
                while (listOfPred.Count > 1)
                {
                    listOfPred[0] = $"&({listOfPred[0]},{listOfPred[1]})";
                    listOfPred.RemoveAt(1);
                }
                propositions.Add(listOfPred[0]);
            }

            // Disjunct all combined props
            while (propositions.Count > 1)
            {
                propositions[0] = $"|({propositions[0]},{propositions[1]})";
                propositions.RemoveAt(1);
            }

            string dnfExpression = propositions[0];

            // Get Hashcode for the created DNF expression
            List<Token> parsedString = dnfExpression.ParseLogicalProposition();
            TableScheme tableScheme = ConstructTableScheme(parsedString);
            string dnfHashCode = tableScheme.TableSchemeToHashCode();

            result = new Tuple<string, string>(dnfExpression, dnfHashCode);

            return result;
        }



        /// <summary>
        /// Convert a table scheme to a CNF expression
        /// </summary>
        /// <param name="tblScheme"> Table scheme which is converted to CNF</param>
        /// <returns> Tuple, with Item1 being the CNF expression as a string and Item2 is the Hashcode resulting from the CNF </returns>
        public static Tuple<string, string> GetCnf(TableScheme tblScheme)
        {
            Tuple<string, string> result = null;
            List<DataRow> truthRows = tblScheme.DataRows.Where(x => x.Result).ToList();
            List<DataRow> falseRows = tblScheme.DataRows.Where(x => x.Result == false).ToList();

            // Check for Tautology or Contradiction
            if (truthRows.Count == 0) return new Tuple<string, string>("0", tblScheme.TableSchemeToHashCode());
            if (falseRows.Count == 0) return new Tuple<string, string>("1", tblScheme.TableSchemeToHashCode());

            List<List<string>> predicateEq = new List<List<string>>();
            foreach (DataRow falseRow in falseRows)
            {
                List<string> transformedPredicates = new List<string>();
                for (var i = 0; i < falseRow.Values.Count; i++)
                {
                    string predicateValue = falseRow.Values[i];
                    string predicate = tblScheme.Predicates[i];

                    switch (predicateValue)
                    {
                        case "1":
                            transformedPredicates.Add($"~({predicate})");
                            break;
                        case "0":
                            transformedPredicates.Add(predicate);
                            break;
                        case "*":
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                }
                predicateEq.Add(transformedPredicates);

            }
            List<string> propositions = new List<string>();

            // Disjunct all predicate evaluations
            foreach (var listOfPred in predicateEq)
            {
                while (listOfPred.Count > 1)
                {
                    listOfPred[0] = $"|({listOfPred[0]},{listOfPred[1]})";
                    listOfPred.RemoveAt(1);
                }
                propositions.Add(listOfPred[0]);
            }

            // Conjunct all combined props
            while (propositions.Count > 1)
            {
                propositions[0] = $"&({propositions[0]},{propositions[1]})";
                propositions.RemoveAt(1);
            }

            string cnfExp = propositions[0];

            // Get Hashcode for the created DNF expression
            List<Token> parsedString = cnfExp.ParseLogicalProposition();
            TableScheme tableScheme = ConstructTableScheme(parsedString);
            string cnfHashCode = tableScheme.TableSchemeToHashCode();

            result = new Tuple<string, string>(cnfExp, cnfHashCode);

            return result;
        }

    }
    public class TableScheme
    {
        public List<string> TableHeaders { get; set; } = new List<string>();
        public List<string> Predicates { get; set; } = new List<string>();
        public List<DataRow> DataRows { get; set; } = new List<DataRow>();
        public int NrOfDataRows => DataRows.Count;
        public int NrOfPredicates => DataRows.Count == 0 ? 0 : DataRows[0].Values.Count;
    }
    public class DataRow
    {
        public int RowNum { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public bool Result { get; set; }

    }
}
