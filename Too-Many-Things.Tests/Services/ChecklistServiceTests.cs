using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Services;
using Xunit;
using Moq;
using Too_Many_Things.Models;
using Autofac.Extras.Moq;
using System.Collections.ObjectModel;

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

        public ObservableCollection<Checklist> GetSampleChecklistList()
        {
            var itemOne = (new Checklist { Name = "Groceries", SortOrder = 1, ChecklistId = 0});
            var itemTwo = (new Checklist { Name = "Chores", SortOrder = 2, ChecklistId = 1 });
            var itemThree = (new Checklist { Name = "Travel-bag", SortOrder = 3, ChecklistId = 2 });

            itemOne.Entry.Add(new Entry { Name = "Milk", EntryId = 1, SortOrder = 1, IsChecked = false});
            itemOne.Entry.Add(new Entry { Name = "Sugar", EntryId = 2, SortOrder = 2, IsChecked = false });
            itemOne.Entry.Add(new Entry { Name = "Oats", EntryId = 3, SortOrder = 3, IsChecked = false });
            itemOne.Entry.Add(new Entry { Name = "Dark Chocolate", EntryId = 4, SortOrder = 4, IsChecked = false });

            var list = new ObservableCollection<Checklist>();
            list.Add(itemOne);
            list.Add(itemTwo);
            list.Add(itemThree);

            return list;
        }
    }
}
