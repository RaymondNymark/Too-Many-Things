using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Services;
using Xunit;
using Moq;
using Too_Many_Things.Models;
using Autofac.Extras.Moq;

namespace Too_Many_Things.Tests
{
    public class ChecklistServiceTests
    {
        [Theory]
        [InlineData("[c][h][e][c][k][l][i][s][t]")]
        [InlineData("                           ")]
        [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890123456789")]
        public void ValidateChecklist_InvalidInputShouldReturnFalse(string name)
        {
            var input = new Checklist() { Name = name };
            var actual = ChecklistService.ValidateChecklist(input);

            Assert.False(actual);
        }
    }
}
