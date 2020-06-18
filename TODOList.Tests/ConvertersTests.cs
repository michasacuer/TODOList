using System;
using System.Windows;
using Xunit;

namespace TODOList.Tests
{
    public class ConvertersTests
    {
        [Fact]
        public void booleanToVisibilityConverter_Test()
        {
            BooleanToVisibilityConverter converter = new BooleanToVisibilityConverter();

            Assert.Equal(Visibility.Visible, converter.Convert(true, null, null, null));
            Assert.Equal(Visibility.Hidden, converter.Convert(false, null, null, null));
            Assert.Equal(true, converter.ConvertBack(Visibility.Visible, null, null, null));
            Assert.Equal(false, converter.ConvertBack(Visibility.Hidden, null, null, null));
        }

        [Fact]
        public void DateConverter_Test()
        {
            DateConverter converter = new DateConverter();

            Assert.Null(converter.Convert(null, null, null, null));
            Assert.Equal(DateTime.Now.ToShortDateString(), converter.Convert(DateTime.Now, null, null, null));
            Assert.Equal(DateTime.Now.AddDays(2).ToShortDateString(), converter.Convert(DateTime.Now.AddDays(2), null, null, null));
            Assert.Equal(DateTime.Now.AddHours(5).ToShortDateString(), converter.Convert(DateTime.Now.AddHours(5), null, null, null));
        }

        [Fact]
        public void IndexToEnableConverter_Test()
        {
            IndexToEnableConverter converter = new IndexToEnableConverter();

            Assert.Equal(true, converter.Convert(0, null, null, null));
            Assert.Equal(false, converter.Convert(-8, null, null, null));
            Assert.Equal(false, converter.Convert(-10, null, null, null));
            Assert.Equal(true, converter.Convert(5, null, null, null));
        }
        [Fact]
        public void InverseBooleanConverter_Test()
        {
            InverseBooleanConverter converter = new InverseBooleanConverter();

            Assert.Equal(false, converter.Convert(true, null, null, null));
            Assert.Equal(true, converter.Convert(false, null, null, null));
        }
    }
}
