using NSubstitute;
using Library.ApplicationCore;
using Library.ApplicationCore.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace UnitTests.Infrastructure.JsonLoanRepositoryTests;

public class GetLoanTest
{
    private readonly ILoanRepository _mockLoanRepository;
    private readonly JsonLoanRepository _jsonLoanRepository;
    private readonly IConfiguration _configuration;
    private readonly JsonData _jsonData;

    public GetLoanTest()
    {
        _mockLoanRepository = Substitute.For<ILoanRepository>();
        _configuration = new ConfigurationBuilder().Build();
        _jsonData = new JsonData(_configuration);
        _jsonLoanRepository = new JsonLoanRepository(_jsonData);
    }

    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Retorna empréstimo quando o ID existe")]
    public async Task GetLoan_RetornaEmprestimoQuandoIdExiste()
    {
        // Arrange
        var loanId = 1; // Use um ID que exista no arquivo Loans.json
        // O JsonData deve estar configurado para acessar os dados reais do JSON
        // Act
        var loan = await _jsonLoanRepository.GetLoan(loanId);

        // Assert
        Assert.NotNull(loan);
        Assert.Equal(loanId, loan?.Id);
    }

    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Retorna null quando o ID não é encontrado")]
    public async Task GetLoan_ReturnsNullWhenLoanIdIsNotFound()
    {
        // Arrange
        var loanId = -1; // Use um ID que não existe no arquivo Loans.json

        // Act
        var loan = await _jsonLoanRepository.GetLoan(loanId);

        // Assert
        Assert.Null(loan);
    }

    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Deve retornar o empréstimo correto quando o ID existe (mock vs real)")]
    public async Task GetLoan_DeveRetornarEmprestimoCorreto_QuandoIdExiste()
    {
        // Arrange
        var loanId = 1; // Certifique-se que este ID existe no Loans.json
        var expectedLoan = new Loan { Id = loanId, /* outros campos se necessário */ };
        _mockLoanRepository.GetLoan(loanId).Returns(expectedLoan);

        // Act
        var actualLoan = await _jsonLoanRepository.GetLoan(loanId);

        // Assert
        Assert.NotNull(actualLoan);
        Assert.Equal(expectedLoan.Id, actualLoan?.Id);
    }
}
