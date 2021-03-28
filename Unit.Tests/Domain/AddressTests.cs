using Common.Tests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Unit.Tests.Domain
{
    [Collection(nameof(AddressFixtureCollection))]
    public class AddressTests : IClassFixture<AddressFixture>
    {
        private readonly AddressFixture fix;
        public AddressTests(AddressFixture fixture)
        {
            fix = fixture;
        }

        [Fact]
        [Trait("Address", "Address_MaxLength")]
        public void Address_CheckForMaxLength()
        {
            var address = fix.MaxLength();
            var validation = address.Valido();

            validation.Should().BeFalse(because: "the max character limits have been exceeded.");
            address.ErrorMessages.Should().HaveCount(4, because: "4 fields exceeded the character limit.");
            address.ErrorMessages.Should().Contain("O campo Endereço deve possuir no máximo 250 caracteres", because: "the address exceeded the character limit.");
            address.ErrorMessages.Should().Contain("O campo Cidade deve possuir no máximo 150 caracteres", because: "the city field exceeded the limit of 150 characters.");
            address.ErrorMessages.Should().Contain("O campo Complemento deve possuir no máximo 250 caracteres", because: "the field for additional address info exceeded the character limit.");
            address.ErrorMessages.Should().Contain("O campo Número deve possuir no máximo 6 caracteres", because: "the number field exceed the limit of 6 characters.");
        }

        [Fact]
        [Trait("Address", "Address_EmptyDate")]
        public void Address_CheckForEmptyData()
        {
            var address = fix.Empty();
            var validation = address.Valido();

            validation.Should().BeFalse(because: "a lot of fields that shouldn't be are empty");
            address.ErrorMessages.Should().HaveCount(6, because: "precisely 6 fields are empty");
            address.ErrorMessages.Should().Contain("O campo Cidade deve ser preenchido", because: "the city is empty.");
            address.ErrorMessages.Should().Contain("O campo Endereço deve ser preenchido", because: "the address is empty");
            address.ErrorMessages.Should().Contain("O campo CEP deve ser preenchido", because: "the zipcode is empty.");
            address.ErrorMessages.Should().Contain("Campo Estado inválido", because: "the state field is empty");
            address.ErrorMessages.Should().Contain("O campo Telefone deve ser preenchido", because: "the phone number is missing");
            address.ErrorMessages.Should().Contain("O campo Número deve ser preenchido", because: "the number field is empty");
        }

        [Fact]
        [Trait("Address", "Address_InvalidFields")]
        public void Address_CheckForInvalidFields()
        {
            var address = fix.Valid(false, false, false);
            var validation = address.Valido();

            validation.Should().BeFalse(because: "the fields are not valid.");
            address.ErrorMessages.Should().HaveCount(3, because: "3 fields are invalid.") ;
            address.ErrorMessages.Should().Contain("Campo CEP inválido", because: "the cep/zipcode is not valid");
            address.ErrorMessages.Should().Contain("Campo Estado inválido", because: "the state is invalid");
            address.ErrorMessages.Should().Contain("Campo Telefone inválido", because: "the phone number is just wrong");
        }

        [Fact]
        [Trait("Address", "Address_EverythingIsAllright")]
        public void Address_CheckForValidEntry()
        {
            var address = fix.Valid();
            var validation = address.Valido();

            validation.Should().BeTrue(because: "all fields are valid.");
            address.ErrorMessages.Should().HaveCount(0, because: "nothing is wrong.");
        }
    }
}
