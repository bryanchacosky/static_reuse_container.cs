public class static_reuse_container<T> : IDisposable, IList<T> {
    // collection of static instances of the containers
    private static List<static_reuse_container<T>> instances =
               new List<static_reuse_container<T>>();

    // internal properties
    private List<T> container = new List<T>();
    private bool    in_use    = false;

    // no public construction; use get_instance()
    private static_reuse_container () {}

    // returns next available reuse container instance, or creates a new one if needed
    public static static_reuse_container<T> get_instance () {
        static_reuse_container<T> instance = instances.FirstOrDefault((it) => it.in_use == false);
        if (instance == null) { instance = new static_reuse_container<T>(); instances.Add(instance); }
        instance.in_use = true;
        return instance;
    }

    // removes any excess containers not currently in use; returns removed count
    public static int trim_excess () { return instances.RemoveAll((it) => it.in_use == false); }

    // override IDisposable
    public void Dispose () { Clear(); this.in_use = false; }

    // override IList<T>
    public T this[int index] { get { return this.container[index]; } set { this.container[index] = value; } }
    public bool IsReadOnly { get { return false; } }
    public int Count { get { return this.container.Count; } }
    public void Add (T item) { this.container.Add(item); }
    public int IndexOf (T item) { return this.container.IndexOf(item); }
    public void Insert (int index, T item) { this.container.Insert(index, item); }
    public void RemoveAt (int index) { this.container.RemoveAt(index); }
    public void Clear () { this.container.Clear(); }
    public bool Contains (T item) { return this.container.Contains(item); }
    public void CopyTo (T[] array, int length) { this.container.CopyTo(array, length); }
    public bool Remove (T item) { return this.container.Remove(item); }
    public IEnumerator<T> GetEnumerator () { return this.container.GetEnumerator(); }
    private IEnumerator GetEnumerator_private () { return this.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator () { return GetEnumerator_private(); }
}

public static class Assert {
    public static bool IsTrue (bool condition, string error) {
        if (condition) return true;
        Console.WriteLine(string.Format("[ASSERT] {0}", error));
        return false;
    }
}
    
class snippet {
    static void Main () {
        using (var reuse_container = static_reuse_container<int>.get_instance()) {
            reuse_container.Add(1);
            reuse_container.Add(2);
            reuse_container.Add(3);
            // ...
        }
        
        using (var reuse_container_a = static_reuse_container<int>.get_instance()) {
            using (var reuse_container_b = static_reuse_container<int>.get_instance()) {
                using (var reuse_container_c = static_reuse_container<int>.get_instance()) {
                    reuse_container_a.Add(4);
                    reuse_container_b.Add(5);
                    reuse_container_c.Add(6);
                    // ...
                }
            }
        }
        
        int removed = static_reuse_container<int>.trim_excess();
        Assert.IsTrue(removed == 3, string.Format("unexpected removed count [removed={0}]", removed));
    }
}
