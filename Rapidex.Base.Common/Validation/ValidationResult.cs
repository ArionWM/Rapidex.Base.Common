using System;
using System.Collections.Generic;
using System.Text;

namespace Rapidex
{
    public class ValidationResult : IValidationResult //From ProCore
    {
        public static ValidationResult Ok { get { return new ValidationResult(); } }

        public List<IValidationResultItem> Errors { get; protected set; } = new List<IValidationResultItem>();

        public List<IValidationResultItem> Warnings { get; protected set; } = new List<IValidationResultItem>();

        public List<IValidationResultItem> Infos { get; protected set; } = new List<IValidationResultItem>();

        public bool Success { get; set; } = true;
        public string Description { get; set; }




    }
}
