//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//

namespace EMP.Forms
{
    public interface IValidationHandler
    {
        void HandleValidationIssues(Validator.Issue[] issues);
    }
}
