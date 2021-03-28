using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;
using Xunit;

namespace Common.Tests
{
    [CollectionDefinition(nameof(AddressFixtureCollection))]
    public class AddressFixtureCollection : ICollectionFixture<AddressFixture> { }

    public class AddressFixture
    {
        public EnderecoViewModel ValidVM()
        {
            Faker<EnderecoViewModel> faker = new Faker<EnderecoViewModel>("pt_BR");

            faker.CustomInstantiator(x =>
                new EnderecoViewModel
                {
                    CEP = "04058-000",
                    Cidade = x.Address.City(),
                    Estado = x.Address.StateAbbr(),
                    TextoEndereco = x.Address.StreetAddress(),
                    Telefone = x.Phone.PhoneNumber("0000000000"),
                    Numero = x.Address.BuildingNumber(),
                    Complemento = x.Lorem.Sentence(3)
                }
            );
            EnderecoViewModel evm = faker.Generate();

            return evm;
        }

        public List<Endereco> Valid(int quantity)
        {
             return Valid(quantity, true, true, true);
        }
        public List<Endereco> Valid(int quantity, bool isZipcodeValid = true, bool isPhoneValid = true, bool isStateValid = true)
        {
            Faker<Endereco> faker = new Faker<Endereco>("pt_BR");

            faker.CustomInstantiator(x =>
                new Endereco(
                    id: Guid.NewGuid(),
                    cep: isZipcodeValid ? "04058-000" : "12345-1234",
                    textoEndereco: x.Address.StreetAddress(),
                    cidade: x.Address.City(),
                    estado: isStateValid ? x.Address.StateAbbr() : x.Address.State(),
                    complemento: x.Lorem.Sentence(3),
                    telefone: isPhoneValid ? x.Phone.PhoneNumber("00000000000") : x.Phone.PhoneNumber("100000000000"),
                    numero: "999"
            ));

            List<Endereco> addresses = faker.Generate(quantity);

            return addresses;
        }

        public Endereco Valid()
        {
            return Valid(1).First();
        }
        public Endereco Valid(bool isZipcodeValid = true, bool isPhoneValid = true, bool isStateValid = true)
        {
            return Valid(1, isZipcodeValid, isPhoneValid,  isStateValid).First();
        }

        public Endereco Empty()
        {
            Endereco person = new Endereco(Guid.Empty, "", "", "", "", "", "", "");
            return person;
        }

        public Endereco MaxLength()
        {
            string longText = string.Concat(Enumerable.Repeat("X", 300));

            Faker<Endereco> faker = new Faker<Endereco>("pt_BR");

            faker.CustomInstantiator(x =>
                new Endereco(
                    id: Guid.NewGuid(),
                    cep: "04058-000",
                    textoEndereco: longText,
                    cidade: longText,
                    estado: x.Address.StateAbbr(),
                    complemento: longText,
                    telefone: "11912345678",
                    numero: "1234567"
            ));

            Endereco address= faker.Generate();

            return address;
        }
    }
}
