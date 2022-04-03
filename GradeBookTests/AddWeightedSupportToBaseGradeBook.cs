using System;
using System.IO;
using System.Linq;
using GradeBook.GradeBooks;
using GradeBook.UserInterfaces;
using Xunit;

namespace GradeBookTests
{
    public class AddWeightedSupportToBaseGradeBook
    {
        
        [Fact(DisplayName = "Create IsWeighted Property Test @add-isweighted-to-basegradebook")]
        public void CreateIsWeightedTest()
        {
            var isWeightedProperty = typeof(BaseGradeBook).GetProperty("IsWeighted");

            Assert.True(isWeightedProperty != null, "`GradeBook.GradeBooks.BaseGradeBook` does not contain an `IsWeighted` property.");
            Assert.True(isWeightedProperty.GetGetMethod().IsPublic == true, "`GradeBook.GradeBooks.BaseGradeBook`'s `IsWeighted` property is not public.");
            Assert.True(isWeightedProperty.PropertyType == typeof(bool), "`GradeBook.GradeBooks.BaseGradeBook`'s `IsWeighted` is not of type `bool`.");
        }

        [Fact(DisplayName = "Refactor GradeBooks and StartingUserInterface Test @refactor-to-support-isweighted")]
        public void RefactorGradeBooksAndStartingUserInterface()
        {
            
            var standardGradeBook = TestHelpers.GetUserType("GradeBook.GradeBooks.StandardGradeBook");
            Assert.True(standardGradeBook != null, "`StandardGradeBook` wasn't found in the `GradeBooks.GradeBook` namespace.");

           
            var constructor = standardGradeBook.GetConstructors().FirstOrDefault();

           
            var parameters = constructor.GetParameters();

            
            Assert.True(parameters.Count() == 2 && parameters[0].ParameterType == typeof(string) && parameters[1].ParameterType == typeof(bool), "`GradeBook.GradeBooks.BaseGradeBook`'s constructor doesn't have the correct parameters. It should be a `string` and then a `bool`.");

            
        }


       
        [Fact(DisplayName = "Set IsWeighted In BaseGradeBook Constructor Test @set-isweighted-property")]
        public void SetIsWeightedInBaseGradeBookConstructorTest()
        {
            // get standardgradebook type
            var standardGradeBook = TestHelpers.GetUserType("GradeBook.GradeBooks.StandardGradeBook");
            Assert.True(standardGradeBook != null, "`StandardGradeBook` wasn't found in the `GradeBooks.GradeBook` namespace.");

            // Instantiate StandardGradeBook with weighted grading
            object gradeBook = Activator.CreateInstance(standardGradeBook, "WeightedTest", true);

            // Assert that is weighted is true
            Assert.True(gradeBook.GetType().GetProperty("IsWeighted").GetValue(gradeBook).ToString().ToLower() == "true", "`GradeBook.GradeBooks.BaseGradeBook`'s constructor didn't properly set the `IsWeighted` property based on the provided bool parameter");
        }

        
        [Fact(DisplayName = "Update StartingUserInterface CreateCommand Methods Condition @add-weighted-to-createcommand")]
        public void UpdateStartingUserInterfacesCreateCommandMethodsCondition()
        {
            
            var output = string.Empty;

            try
            {
                using (var consoleInputStream = new StringReader("close"))
                {
                    Console.SetIn(consoleInputStream);
                    using (var consolestream = new StringWriter())
                    {
                        Console.SetOut(consolestream);
                        StartingUserInterface.CreateCommand("create test standard");
                        output = consolestream.ToString().ToLower();

                        
                        Assert.True(output.Contains("command not valid"), "`GradeBook.UserInterfaces.StartingUserInterface` didn't write a message to the console when the create command didn't contain a name, type, and if it was weighted (true / false).");

                        
                        Assert.True(output.Contains("command not valid, create requires a name, type of gradebook, if it's weighted (true / false)."), "`GradeBook.UserInterfaces.StartingUserInterface` didn't write 'Command not valid, Create requires a name, type of gradebook, if it's weighted (true / false)..' to the console when the create command didn't contain both a name and type.");

                       
                        Assert.True(!output.Contains("created gradebook"), "`GradeBook.UserInterfaces.StartingUserInterface` still created a gradebook when the create command didn't contain a name, type, if it's weighted (true / false).");
                    }
                }
            }
            finally
            {
                StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
                Console.SetOut(standardOutput);
                StreamReader standardInput = new StreamReader(Console.OpenStandardInput());
                Console.SetIn(standardInput);
            }

            output = string.Empty;

            try
            {
                using (var consoleInputStream = new StringReader("close"))
                {
                    Console.SetIn(consoleInputStream);
                    using (var consolestream = new StringWriter())
                    {
                        Console.SetOut(consolestream);
                        StartingUserInterface.CreateCommand("create test standard true");
                        output = consolestream.ToString().ToLower();

                        Assert.True(output.Contains("standard"), "`GradeBook.UserInterfaces.StartingUserInterface` didn't create a gradebook when the `CreateCommand` included a name, type, and if it was weighted (true / false).");
                    }
                }
            }
            finally
            {
                StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
                Console.SetOut(standardOutput);
                StreamReader standardInput = new StreamReader(Console.OpenStandardInput());
                Console.SetIn(standardInput);
            }
        }
    }
}
