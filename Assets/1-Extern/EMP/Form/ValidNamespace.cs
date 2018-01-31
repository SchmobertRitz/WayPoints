//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using System.Text.RegularExpressions;

namespace EMP.Forms
{
    public class ValidNamespace : Validator<string>
    {
        protected override Issue[] GetValidationIssues(string value)
        {
            Regex patternNamespace = new Regex(@"^\s*[a-zA-Z_][a-zA-Z0-9]*(\.[a-zA-Z_][a-zA-Z0-9]*)*\s*$");
            if (!patternNamespace.IsMatch(value))
            {
                return new Issue[]
                {
                    new Issue
                    {
                        Message = "The value '" + value + "' is not a valid name space.",
                        Severity = ESeverity.Error
                    }
                };
            }
            return NoIssues();
        }
    }
}
