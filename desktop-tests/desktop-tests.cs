using Xunit;


namespace xUnit
{

    public class Get_charger_data
    {
 
        [Fact]
        public void Passing_tests()
        {
            Assert.Equal(4, add(2, 2));
        }
        int add(int x, int y)
        {
            return x + y;
        }
    }
}