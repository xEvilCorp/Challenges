using Bogus;
using System;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;
using Xunit;

namespace Common.Tests
{
    [CollectionDefinition(nameof(DonationFixtureCollection))]
    public class DonationFixtureCollection : ICollectionFixture<DonationFixture>, ICollectionFixture<AddressFixture>, ICollectionFixture<CreditCardFixture> { }

    public class DonationFixture
    {
        public DoacaoViewModel ValidVM()
        {
            Faker<DoacaoViewModel> faker = new Faker<DoacaoViewModel>("pt_BR");

            faker.CustomInstantiator(x => new DoacaoViewModel
            {
                Valor = x.Finance.Amount(5, 500, 2),
                DadosPessoais = new PersonFixture().ValidVM(),
                EnderecoCobranca = new AddressFixture().ValidVM(),
                FormaPagamento = new CreditCardFixture().ValidVM()
               
            });
              
            DoacaoViewModel donationVM = faker.Generate();

            return donationVM;
        }

        public Doacao Valid(bool isEmailInvalid = false, double? value = 10, bool maxLengthField = false, bool aceitaTaxa = false)
        {
            Faker<Doacao> faker = new Faker<Doacao>("pt_BR");

            faker.CustomInstantiator(x => new Doacao(
                id: Guid.NewGuid(),
                dadosPessoaisId: Guid.NewGuid(),
                enderecoCobrancaId: Guid.NewGuid(),
                valor: value ?? Convert.ToDouble(x.Finance.Amount(5, 500, 2)),
                dadosPessoais: new PersonFixture().Valid(isEmailInvalid, maxLengthField),
                formaPagamento: new CreditCardFixture().Valid(),
                enderecoCobranca: new AddressFixture().Valid(),
                aceitaTaxa: aceitaTaxa
            ));

            Doacao donation = faker.Generate();

            return donation;
        }

        public Doacao Empty(bool anonymous = false)
        {
            Faker<Doacao> faker = new Faker<Doacao>("pt_BR");

            faker.CustomInstantiator(x => new Doacao(
                id: Guid.Empty,
                dadosPessoaisId: Guid.Empty,
                enderecoCobrancaId: Guid.Empty,
                valor: 0,
                dadosPessoais: new PersonFixture().Empty(anonymous),
                formaPagamento: new CreditCardFixture().Empty(),
                enderecoCobranca: new AddressFixture().Empty()
            ));

            Doacao donation = faker.Generate();

            return donation;
        }
    }
}
