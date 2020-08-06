# Godot C# Autowire

These are a few AutoWire attributes I wrote for working in C# in Godot.

These are based on loosely on stuff in [AlleyCat] and code that [insraq] had shared.
They gave me the idea and awareness of how to implement, however the implementation
is all mine.

If you know of anything else that could be autowired, let me know. Also, if you
know a way to automatically calla function in `_Ready` for all nodes that enter
the scene, without me having to add it manually, let me know.

## How to use:

Inside of a class, I have this as the very first line:

```csharp
public override void _Ready() => this.AutoWire();
```

This will call autowire, allowing everything to just work.

### GetNode/GetSingleton

The `GetNode` and `GetSingleton` attributes will automatically, determine the path
based on the name of the variable they are assigned to. `GetSingleton` adds `/root/` to
the beginning of the path. Alternatively, you can pass in a path, and that will be used instead.

Example:

```csharp
[GetSingleton] private UI _ui;
[GetSingleton] private GameManager _gameManager;
[GetNode("Cauldron/Liquid")] private AnimatedSprite _liquid;
[GetNode("Cauldron/Liquid/Particles2D")] private Particles2D _bubbles;
[GetNode("Cauldron/Liquid/Light2D")] private Light2D _light;
```

### Preload

The `Preload` attribute automatically runs Load to get the packed scene, and assigns it
to the variable. This makes it easy to interact with packed scenes, just like preloading
in gdscript.

Example:

```csharp
[Preload("res://Scenes/UserInterface/InteractionBubble.tscn")]
private PackedScene InteractionBubble;
```

### Make

The `Make` property instantiates a new node of the type of the variable,
and add's it as a child to the node. Alternatively, you can pass in a scene path
and instantiate that instead.

Example:

```csharp
[Make] private AnimationPlayer _animation;
[Make] private Tween _tween;
[Make("res://Scenes/Wings.tscn")] private Wings _wings;
```

### OnReady

The `OnReady` attribute can be used on methods or properties. If used on
a property, it will assign it the value passed in. This has to be a constant value.
This is the last attribute that is run against properties, prior to any attributes
on methods.

It's mostly useful if you have a property that needs a value set, but it's setter
relies on GetNode/GetSingleton/Preload/Make.

It's alternative use, on a method, runs the method (after all property attributes
are set), so that you can use run things during `_Ready`. The main purpose of this
is so that you can have an easily copy-pastable line for `_Ready` and don't have
to do more inside your ready function.

Example:

```csharp
/// I don't have a good example of this...
[OnReady] public int MyProperty { get; set; }

[OnReady]
private void _SetUp()
{
    _cauldronUI.Visible = false;
    _liquid.Visible = false;
    _bubbles.Visible = false;
}
```

### Connect

The `Connect` attribute allows you to connect a method to a signal on the same
or another node. You can pass in just the signal name, setting the path to the same
node; The node path and the signal name; The node path, signal name, and some ConnectFlags;
The node path, signal name, a Godot.Collections.Array, and the ConnectFlags.

Example:

```csharp
[Connect("MagnetismRadius", "body_entered")]
public void OnMagnetismRadiusBodyEntered(Node2D body)
{
    // Do stuff...
}

// Connect to a signal on the same node
[Connect(nameof(Harvested))]
public void OnHarvest()
{
    // ..
}

// With ConnectFlags
[Connect(UI.ROOT_PATH, nameof(UI.UiClosed), ConnectFlags.Deferred)]
public void OnClose()
{
    // ...
}
```

[AlleyCat]: https://github.com/mysticfall/AlleyCat
[insraq]: https://github.com/insraq/shotcaller
