using System.Data;
using System.Net;
using Dapper;
using loginApi.Repos;
using loginApi.Services;
using Moq;
using NUnit.Framework.Internal;

namespace LoginApiTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task UsernameNotExistShouldCreateSuccessAndReturnOk()
    {

    }
}