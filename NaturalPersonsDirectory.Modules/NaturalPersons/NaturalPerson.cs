﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using NaturalPersonsDirectory.Common;
using NaturalPersonsDirectory.Models;
using System.Collections.Generic;
using System.Linq;

namespace NaturalPersonsDirectory.Modules
{
    public class NaturalPersonRequest
    {
        public string FirstNameGE { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameGe { get; set; }
        public string LastNameEn { get; set; }
        public string PassportNumber { get; set; }
        public string Birthday { get; set; }
        public string Address { get; set; }
        public string ContactInformations { get; set; }
    }

    public class RelatedPerson : NaturalPerson
    {
        public string RelationType { get; set; }
    }

    public class NaturalPersonResponse
    {
        private int? _count = default;
        public int Count
        {
            get 
            {
                if (_count == null)
                {
                    return NaturalPersons == null ? 0 : NaturalPersons.Count();
                }
                else
                {
                    return _count.Value;
                }
            }
            set { _count = value; }
        }
        public IEnumerable<NaturalPerson> NaturalPersons { get; set; }
    }

    public class RelatedPersonsResponse
    {
        private int? _count = default;
        public int Count
        {
            get
            {
                if (_count == null)
                {
                    return RelatedPersons == null ? 0 : RelatedPersons.Count();
                }
                else
                {
                    return _count.Value;
                }
            }
            set { _count = value; }
        }

        public IEnumerable<RelatedPerson> RelatedPersons { get; set; }
    }

    public class NaturalPersonRequestValidator : AbstractValidator<NaturalPersonRequest>
    {
        public NaturalPersonRequestValidator()
        {
            RuleFor(request => request.FirstNameEn).NotEmpty().WithMessage("FirstNameEn should not be empty.");
            RuleFor(request => request.FirstNameGE).NotEmpty().WithMessage("FirstNameGe should not be empty.");
            RuleFor(request => request.LastNameEn).NotEmpty().WithMessage("LastNameEn should not be empty.");
            RuleFor(request => request.LastNameGe).NotEmpty().WithMessage("LastNameGe should not be empty.");
            RuleFor(request => request.Address).NotEmpty().WithMessage("Address should not be empty.");
            RuleFor(request => request.PassportNumber).NotEmpty().Matches("^[0-9]{11}").WithMessage("Passport number should be 11 characters long and contain only digits");
            RuleFor(request => request.Birthday).NotEmpty().Must(ValidateFormat.ValidDate).WithMessage("Date should be in valid form");
            RuleFor(request => request.ContactInformations)
                .NotEmpty()
                .Must(ValidateFormat.ValidContactInformations)
                .WithMessage(
                "Contact informations format is incorrect. " +
                "Contact information should be phone number (pattern +995-5XX-XXX-XXX) or email address. " +
                "Multiple informations should be separated by comma (,).");
        }
    }
}
