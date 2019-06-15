# static_reuse_container.cs

C# snippet of a flexible system for reusing static containers. In the current project, it's frequent that we have static containers in individual classes used for queries -- preventing the need to allocate/deallocate containers each frame -- but these containers are manually managed and static in the class. `static_reuse_container<T>` creates an easier to manage approach and shares containers by type when available.

## usage
```c#
static void Main () {
    using (var reuse_container = static_reuse_container<int>.get_instance()) {
        reuse_container.Add(1);
        reuse_container.Add(2);
        reuse_container.Add(3);
        // ...
    }
}
```
