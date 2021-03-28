using Common.Tests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Unit.Tests.Domain
{
    [Collection(nameof(CreditCardFixtureCollection))]
    public class CreditCardTests : IClassFixture<CreditCardFixture>
    {
        private readonly CreditCardFixture fix;
        public CreditCardTests(CreditCardFixture fixture)
        {
            fix = fixture;
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_Valid")]
        public void CreditCard_CheckForValidEntry()
        {
            var creditCard = fix.Valid();
            var validation = creditCard.Valido();

            validation.Should().BeTrue(because: "all fields have been correctly filled.");
            creditCard.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_EmptyData")]
        public void CreditCard_CheckForEmptyData()
        {
            var creditCard = fix.Empty();
            var validation = creditCard.Valido();

            validation.Should().BeFalse(because: "there are fields empty.");
            creditCard.ErrorMessages.Should().HaveCount(4, because: "4 required fields are empty.");
            creditCard.ErrorMessages.Should().Contain("O campo CVV deve ser preenchido", because: "the card cvv field is required but it's empty.");
            creditCard.ErrorMessages.Should().Contain("O campo Validade deve ser preenchido", because: "the card expiration date field is required but it's missing.");
            creditCard.ErrorMessages.Should().Contain("O campo Nome Titular deve ser preenchido", because: "the name field is required but it's empty.");
            creditCard.ErrorMessages.Should().Contain("O campo Número de cartão de crédito deve ser preenchido", because: "the card number field is required but it's empty.");

        }

        [Fact]
        [Trait("CreditCard", "CreditCard_MultipleFieldsInvalid")]
        public void CreditCard_CheckForInvalidFields()
        {
            var creditCard = fix.Invalid();
            var validation = creditCard.Valido();

            validation.Should().BeFalse(because: "there are invalid fields.");
            creditCard.ErrorMessages.Should().HaveCount(3, because: "3 fields are invalid.");
            creditCard.ErrorMessages.Should().Contain("Campo Data de Vencimento do cartão de crédito inválido", because: "the expire date is impossible");
            creditCard.ErrorMessages.Should().Contain("Campo Número de cartão de crédito inválido", because: "the card number is invalid");
            creditCard.ErrorMessages.Should().Contain("Campo CVV inválido", because: "the card cvv is not valid");
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_Expired")]
        public void CreditCard_CheckForCardExpired()
        {
            var creditCard = fix.WithExpiredDate();
            var validation = creditCard.Valido();

            validation.Should().BeFalse(because: "there should be an error");
            creditCard.ErrorMessages.Should().Contain("Cartão de Crédito com data expirada", because: "the card has expired");
            creditCard.ErrorMessages.Should().HaveCount(1, because: "the only error is the card date being expired");
        }


        [Fact]
        [Trait("CreditCard", "CreditCard_MaxLength")]
        public void CreditCard_CheckForMaxLength()
        {
            var creditCard = fix.InvalidCardOwnerNameMaxLength();
            var validation = creditCard.Valido();

            validation.Should().BeFalse(because: "the character limit has been exceeded.");
            creditCard.ErrorMessages.Should().Contain("O campo Nome Titular deve possuir no máximo 150 caracteres", because: "the card owner name field has exceed the limit of 150 characters.");
            creditCard.ErrorMessages.Should().HaveCount(1, because: "only the card owner name field has exceed the character limit");

        }
    }
}
