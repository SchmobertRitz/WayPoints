//
// MIT License
// Copyright (c) EMP - https://github.com/SchmobertRitz/EMP-Unity
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Logger definition here

namespace EMP.Forms
{
    public abstract class Validator
    {
        protected IValidationHandler handler;

        public class Null : Validator
        {
            protected override Issue[] GetValidationIssues(object value)
            {
                return new Issue[0];
            }
        }

        public enum ESeverity
        {
            Info, Waring, Error
        }

        public struct Issue
        {
            public ESeverity Severity;
            public string Message;
        }

        protected Issue[] NoIssues()
        {
            return new Issue[0];
        }

        public Validator Bind(IValidationHandler handler)
        {
            this.handler = handler;
            return this;
        }

        public Issue[] Validate(object value)
        {
            Issue[] issues = GetValidationIssues(value);
            if (handler != null)
            {
                handler.HandleValidationIssues(issues);
            }
            return issues;
        }

        protected abstract Issue[] GetValidationIssues(object value);
    }

    public abstract class Validator<T> : Validator
    {
        protected override Issue[] GetValidationIssues(object value)
        {
            return GetValidationIssues((T)value);
        }

        protected abstract Issue[] GetValidationIssues(T value);
    }

}
