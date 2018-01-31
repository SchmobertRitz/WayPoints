//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMP.Forms
{
    public class ValidationLabel : Label, IValidationHandler
    {

        public ValidationLabel(string text) : base(text) { }

        // Start coding here
        public void HandleValidationIssues(Validator.Issue[] issues)
        {
            Text = "";
            if (issues != null && issues.Length > 0)
            {
                List<Validator.Issue> issuesList = new List<Validator.Issue>(issues);
                issuesList.Sort((i1, i2) => i1.Severity - i2.Severity);
                Text = issuesList[0].Message;
            }
        }
    }
}
