using psychotest.Model;
using psychotest_web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace psychotest_webTests.Controllers
{
    class HomeControllerTests
    {
        public async Task<IdValidationResult> ValidateID(string id)
        {
            if(id == null)
                return IdValidationResult.IdIncorrect;

            if (!Regex.IsMatch(id, @"^\d{10}$"))
                return IdValidationResult.IdIncorrect;

            try
            {
                var probe = await Database.GetProbe(id);
                if (probe.Done)
                    return IdValidationResult.IdUsed;
                else
                    return IdValidationResult.Valid;
            }
            catch (ArgumentOutOfRangeException)
            {
                return IdValidationResult.IdIncorrect;
            }
            catch (Exception)
            {
                throw;
            }

        }
        [TestCase("", IdValidationResult.IdIncorrect)]
        [TestCase("0001531998", IdValidationResult.Valid)]
        [TestCase("0055350268", IdValidationResult.IdUsed)]
        [TestCase("0000000000", IdValidationResult.IdIncorrect)]
        [TestCase("000000000x", IdValidationResult.IdIncorrect)]

        public async Task ValidateID_testNormal(string id, IdValidationResult expected)
        {
            Assert.AreEqual(expected, await ValidateID(id));
        }

        [TestCase("0001531998")]
        public  void ValidateID_testConnectionIssue(string id)
        {
            Assert.That(async () => await ValidateID(id), Throws.Exception);
        }
    }
}
