using ObjCRuntime;

namespace MonoTouch
{
	class ObjCRuntime
	{
		internal class Selector : global::ObjCRuntime.Selector
		{
			public Selector(string name) : base(name)
			{
			}
		}
	}
}