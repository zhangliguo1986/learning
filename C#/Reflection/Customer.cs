

namespace Reflection
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string lastName { get; set; }

        public string Address { get; set; }

        public Customer() { }

        public bool Validate(Customer customerObj)
        {
            return true;
        }
    }
}