using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;
using Xunit;

namespace Common.Tests
{
    [CollectionDefinition(nameof(PersonFixtureCollection))]
    public class PersonFixtureCollection : ICollectionFixture<PersonFixture> { }

    public class PersonFixture
    {
        public PessoaViewModel ValidVM(bool isEmailInvalid = false)
        {
            Faker<PessoaViewModel> faker = new Faker<PessoaViewModel>("pt_BR");

            faker.CustomInstantiator(x =>
                new PessoaViewModel { 
                    Nome = x.Name.FirstName(),
                    Email = isEmailInvalid ? "Email invalido" : x.Internet.Email()
                }
            );

            PessoaViewModel pessoaVm = faker.Generate();

            return pessoaVm;
        }

        public List<Pessoa> Valid(int quantity, bool isEmailInvalid = false, bool maxLengthField = false)
        {
            Faker<Pessoa> faker = new Faker<Pessoa>();

            faker.CustomInstantiator(x =>
                new Pessoa(
                    id: Guid.NewGuid(),
                    nome: x.Name.FirstName(),
                    email: isEmailInvalid ? "Email Invalido" : x.Internet.Email(),
                    anonima: false,
                    mensagemApoio: maxLengthField ? x.Lorem.Sentence(350) : x.Lorem.Sentence(3)
            ));

            List<Pessoa> people = faker.Generate(quantity);

            return people;
        }

        public Pessoa Valid(bool isEmailInvalid = false, bool maxLengthField = false)
        {
            return Valid(1, isEmailInvalid, maxLengthField).First();
        }

        public Pessoa Empty(bool anonymous = false)
        {
            Pessoa person = new Pessoa(Guid.Empty, "", "", anonymous, "");
            return person; 
        }

        public Pessoa MaxLength()
        {
            string longText = string.Concat(Enumerable.Repeat("X", 160));
            string ultraLongText = string.Concat(Enumerable.Repeat("X", 510));

            Pessoa p = new Pessoa(Guid.NewGuid(), longText, longText, false, ultraLongText);
            return p;
        }
    }
}
