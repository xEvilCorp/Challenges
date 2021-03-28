using Bogus;
using System.Linq;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;
using Xunit;

namespace Common.Tests
{

    [CollectionDefinition(nameof(CreditCardFixtureCollection))]
    public class CreditCardFixtureCollection : ICollectionFixture<CreditCardFixture> { }

    public class CreditCardFixture
    {
        public CartaoCreditoViewModel ValidVM()
        {

            Faker<CartaoCreditoViewModel> faker = new Faker<CartaoCreditoViewModel>();

            faker.CustomInstantiator(x => new CartaoCreditoViewModel {
                NomeTitular =  x.Person.FullName,
                NumeroCartaoCredito = x.Finance.CreditCardNumber(),
                CVV = x.Finance.CreditCardCvv(),
                Validade = "02/25"
            });

            CartaoCreditoViewModel cardVM = faker.Generate();

            return cardVM;
        }

        public CartaoCredito Valid()
        {
            Faker<CartaoCredito> faker = new Faker<CartaoCredito>();

            faker.CustomInstantiator(x => new CartaoCredito(
                nomeTitular: x.Person.FullName,
                numero: x.Finance.CreditCardNumber(),
                cvv: x.Finance.CreditCardCvv(),
                validade: "02/25"
            ));

            CartaoCredito card = faker.Generate();

            return card;
        }

        public CartaoCredito Invalid(bool isCvvInvalid = true, bool isExpDateInvalid = true, bool isNumberInvalid = true)
        {
            Faker<CartaoCredito> faker = new Faker<CartaoCredito>();

            faker.CustomInstantiator(x => new CartaoCredito(
                nomeTitular: x.Person.FullName,
                numero: isNumberInvalid ? "12345678" : x.Finance.CreditCardNumber(),
                cvv: isCvvInvalid ? "123A" : x.Finance.CreditCardCvv(),
                validade: isExpDateInvalid ? "13/25" : "02/25"
            ));

            CartaoCredito card = faker.Generate();

            return card;
        }

        public CartaoCredito WithExpiredDate()
        {
            Faker<CartaoCredito> faker = new Faker<CartaoCredito>();

            faker.CustomInstantiator(x => new CartaoCredito(
                nomeTitular: x.Person.FullName,
                numero: x.Finance.CreditCardNumber(),
                cvv: x.Finance.CreditCardCvv(),
                validade: "02/18"
            ));

            CartaoCredito card = faker.Generate();

            return card;
        }

        public CartaoCredito Empty()
        {
            CartaoCredito card = new CartaoCredito("", "", "", "");

            return card;
        }

        public CartaoCredito InvalidCardOwnerNameMaxLength()
        {
            string longText = string.Concat(Enumerable.Repeat("X", 151));

            Faker<CartaoCredito> faker = new Faker<CartaoCredito>();

            faker.CustomInstantiator(x => new CartaoCredito(
                nomeTitular: longText,
                numero: x.Finance.CreditCardNumber(),
                cvv: x.Finance.CreditCardCvv(),
                validade: "02/25"
            ));

            CartaoCredito card = faker.Generate();

            return card;
        }
    }
}
