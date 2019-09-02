
namespace CSharpLanguage
{
    public interface IFoo<out T>
    {
        T GetName();
    }

    public class Foo : IFoo<string>
    {
        public string GetName()
        {
            return GetType().Name;
        }
    }
}