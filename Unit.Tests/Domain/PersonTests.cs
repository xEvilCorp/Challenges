using Common.Tests;
using Xunit;
using FluentAssertions;

namespace Unit.Tests.Domain
{
    [Collection(nameof(PersonFixtureCollection))]
    public class PersonTests : IClassFixture<PersonFixture>
    {
        private readonly PersonFixture fix;
        public PersonTests(PersonFixture fixture)
        {
            fix = fixture;
        }

        [Fact]
        [Trait("Person", "Person_InvalidEmail")]
        public void Person_CheckInvalidEmail()
        {
            var person = fix.Valid(true);
            var validation = person.Valido();

            validation.Should().BeFalse(because: "the email field is incorrect.");
            person.ErrorMessages.Should().HaveCount(1, because: "only the email field is incorrect.");
            person.ErrorMessages.Should().Contain("O campo Email é inválido.");
        }

        [Fact]
        [Trait("Person", "Person_InvalidCharacterLength")]
        public void Person_CheckMaxLength()
        {
            var person = fix.MaxLength();
            var validation = person.Valido();

            validation.Should().BeFalse(because: "the fields for name, email and message have exceeded the max character limit.");
            person.ErrorMessages.Should().HaveCount(3, because: "name, email and message are invalid.");
            person.ErrorMessages.Should().Contain("O campo Nome deve possuir no máximo 150 caracteres.");
            person.ErrorMessages.Should().Contain("O campo Email deve possuir no máximo 150 caracteres.");
            person.ErrorMessages.Should().Contain("O campo Mensagem de Apoio deve possuir no máximo 500 caracteres.");
        }

        [Fact]
        [Trait("Person", "Person_EmptyData")]
        public void Person_CheckForEmptyData()
        {
            var person = fix.Empty();
            var validation = person.Valido();

            validation.Should().BeFalse(because: "it's not valid as it's empty.");
            person.ErrorMessages.Should().HaveCount(2, because: "both required fields contain nothing.");
            person.ErrorMessages.Should().Contain("O campo Nome é obrigatório.", because: "the name is empty");
            person.ErrorMessages.Should().Contain("O campo Email é obrigatório.", because: "the email is empty");
        }

        [Fact]
        [Trait("Person", "Person_AllFieldsCorrectlyFilled")]
        public void Person_CheckForValidEntry()
        {
            var person = fix.Valid();
            var validation = person.Valido();

            validation.Should().BeTrue(because: "all fields contain proper data.");
            person.ErrorMessages.Should().BeEmpty();
        }
    }
}
