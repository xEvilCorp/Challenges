using Common.Tests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Unit.Tests.Domain
{
    [Collection(nameof(DonationFixtureCollection))]
    public class DonationTests : IClassFixture<DonationFixture>, IClassFixture<AddressFixture>, IClassFixture<CreditCardFixture>
    {

        private readonly DonationFixture donationFix;
        private readonly AddressFixture addressFix;
        private readonly CreditCardFixture cardFix;

        public DonationTests(DonationFixture donFix, AddressFixture addFix, CreditCardFixture caFix)
        {
            donationFix = donFix;
            addressFix = addFix;
            cardFix = caFix;
        }


        [Theory]
        [InlineData(10000.01)]
        [InlineData(10000)]
        [InlineData(5000)]
        [InlineData(4500.1)]
        [Trait("Donation", "Donation_ValueHigherThanLimit")]
        public void Donation_CheckForValueBeyondLimit(double donationValue)
        {
            var donation = donationFix.Valid(false, donationValue);
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeFalse(because: "the value field is invalid.");
            donation.ErrorMessages.Should().Contain("Valor máximo para a doação é de R$4.500,00", because: "The value of the value field is higher than 4500");
            donation.ErrorMessages.Should().HaveCount(1, because: "only the donation value field is invalid");
        }

        [Theory]
        [InlineData(-1000)]
        [InlineData(-1.5)]
        [InlineData(-1)]
        [InlineData(-0.1)]
        [InlineData(0)]
        [Trait("Donation", "Donation_ValueZeroOrLower")]
        public void Donation_CheckForValueZeroOrLower(double donationValue)
        {
            var donation = donationFix.Valid(false, donationValue);
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeFalse(because: "the value field is invalid");
            donation.ErrorMessages.Should().Contain("Valor mínimo de doação é de R$ 5,00", because: "The value field is bellow 5");
            donation.ErrorMessages.Should().HaveCount(1, because: "only the donation value field is invalid");
        }

        [Fact]
        [Trait("Donation", "Donation_MaxLength")]
        public void Donation_CheckForMaxLength()
        {
            var donation = donationFix.Valid(false, null, true);
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeFalse(because: "The message field contains more characters than allowed.");
            donation.ErrorMessages.Should().HaveCount(1, because: "the only invalid field is the message one.");
            donation.ErrorMessages.Should().Contain("O campo Mensagem de Apoio deve possuir no máximo 500 caracteres.", because: "the message field surpassed the 500 character limit.");
        }

        [Fact]
        [Trait("Donation", "Donation_EmptyDataAnonymous")]
        public void Donation_CheckForAnonymousEmptyData()
        {
            var donation = donationFix.Empty(true);
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeFalse(because: "Required fields are empty.");
            donation.ErrorMessages.Should().HaveCount(2, because: "2 fields are empty though required.");
            donation.ErrorMessages.Should().Contain("Valor mínimo de doação é de R$ 5,00", because: "the mininum value for the donation has not been achieved.");
            donation.ErrorMessages.Should().Contain("O campo Email é obrigatório.", because: "the email is empty.");
        }

        [Fact]
        [Trait("Donation", "Donation_EmptyData")]
        public void Donation_CheckForEmptyData()
        {
            var donation = donationFix.Empty(false);
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeFalse(because: "Required fields are empty.");
            donation.ErrorMessages.Should().HaveCount(3, because: "3 fields are empty though required.");
            donation.ErrorMessages.Should().Contain("Valor mínimo de doação é de R$ 5,00", because: "the mininum value for the donation has not been achieved.");
            donation.ErrorMessages.Should().Contain("O campo Nome é obrigatório.", because: "the name is empty.");
            donation.ErrorMessages.Should().Contain("O campo Email é obrigatório.", because: "the email is empty.");
        }

        [Fact]
        [Trait("Donation", "Donation_Valid")]
        public void Donation_CheckForValidEntry()
        {
            var donation = donationFix.Valid();
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeTrue(because: "the fields are all correct");
            donation.ErrorMessages.Should().BeEmpty(because: "there are no errors.");
        }

        [Fact]
        [Trait("Donation", "Donation_ValidWithTaxIncluded")]
        public void Donation_CheckForValidEntryWithTaxIncluded()
        {
            var donation = donationFix.Valid(false, 5, false, true);
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeTrue(because: "the fields are all correct");
            donation.ErrorMessages.Should().BeEmpty(because: "there are no errors.");
            donation.Valor.Should().Be(6, because: "valor com taxa de 20%");
        }

        [Fact]
        [Trait("Donation", "Donation_InvalidEmail")]
        public void Donation_CheckForInvalidEmail()
        {
            var donation = donationFix.Valid(true);
            donation.AdicionarEnderecoCobranca(addressFix.Valid());
            donation.AdicionarFormaPagamento(cardFix.Valid());
            var validation = donation.Valido();

            validation.Should().BeFalse(because: "there are invalid fields.");
            donation.ErrorMessages.Should().Contain("O campo Email é inválido.", because: "the email is incorrect.");
            donation.ErrorMessages.Should().HaveCount(1, because: "only 1 field is incorrect, the email.");
        }


    }
}
