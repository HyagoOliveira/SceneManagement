# Scene Management

* Loads new Scenes using Screen Faders and Loading Scenes.
* Unity minimum version: **2019.3**
* Current version: **1.0.1**
* License: **MIT**
* Dependencies:
    - [com.actioncode.attributes : 2.1.0](https://github.com/HyagoOliveira/Attributes/tree/2.1.0)
    - [com.actioncode.awaitable-coroutines : 1.0.0](https://github.com/HyagoOliveira/AwaitableCoroutines/tree/1.0.0)
    - [com.actioncode.scriptable-settings-provider : 1.0.0](https://github.com/HyagoOliveira/ScriptableSettingsProvider/tree/1.0.0)

## Summary

It's normal for most games to have multiple Scenes. A common user case is to switch between them in a nice way.

This package contains a Scene Manager class which loads new Scenes using customized Screen Faders and Loading Scenes (Scenes created just to show the Loading Progress). 

Also, the current loading scene progress can be shown using a ``TMP_Text``, ```Text``` or ```Slider``` UI component.

![Showcase](/Documentation~/load-scene-showcase.gif "Scene Manager")

## How It Works

In order to use the package, you must first create a SceneManager ScriptableObject asset and use it on your classes. 
You can also create SceneTransition ScriptableObject assets to control how to Loading Process should happen.

## How To Use

### Creating a Scene Manager ScriptableObject

Open the **Project Settings** Windows and select Scene Manager (below ActionCode group).

Click on the **Create** button and save a new Scene Manager asset.

![The Scene Manager Menu](/Documentation~/scene-manager-menu.png "The Scene Manager Menu")

Click on the Create button next to Default Transition to create and assign a new SceneTransiion asset.
You can manually create this asset by going in Asset > Create > ActionCode > SceneManager > Scene Transition. 

### Using Screen Faders

SceneTransition assets have an attribute for a **Screen Fader Prefab**. This prefab must have a component implementing the [AbstractScreenFader](/Runtime/ScreenFaders/AbstractScreenFader.cs) component.

This package already provides two classes implementing this component:

1. [AnimationScreenFader](/Runtime/ScreenFaders/AnimationScreenFader.cs): fades the screen in and out using a local ```Animation``` component. Perfect to use when fading using animations.
2. [CanvasScreenFader](/Runtime/ScreenFaders/CanvasScreenFader.cs): fades the screen in and out using a local ```CanvasGroup``` component. Use this script and set the attributes to choose how to fade a ```Canvas```.

If those components do not meet your specifications, please feel free to create your own fade component and perhaps contribute to the package. :)

Also, there are two prefabs created using those components at the [Prefabs](/Prefabs) folder. You can use those prefabs or create new prefabs variants using them.

### Loading new Scenes

The example bellow shows how to load new scenes using Scene Loading Settings asset:

```csharp
using UnityEngine;
using ActionCode.SceneManagement;

[DisallowMultipleComponent]
public sealed class LoadingTest : MonoBehaviour
{
    [Scene] public string sceneToLoad;
    public SceneManager sceneManager;    

    public async void Load()
    {
        await sceneManager.LoadScene(scenesToLoad);
    }
}
```

In this example, we are

1. Using the [Scene](/Runtime/Attributes/SceneAttribute.cs) attribute on a ```string``` or ```int``` field to display an Object Field for Scene assets.
2. Creating a reference for **SceneManager** asset and using it to load the ```sceneToLoad``` Scene using ```LoadScene()``` function. This is a asynchronous function so you can hold your code at that moment.


### Create Loading Scenes

Inside your Loading Scene, you can use the [LoadingSlider](/Runtime/UI/LoadingSlider.cs) and/or [LoadingText](/Runtime/UI/LoadingText.cs) components to display the current loading progress.

If you want to lock the next scene activation until an action is done, i.e., wait for an input or animation to be completely played inside your Loading Scene, you can use the ```SceneManager.LockLoading()``` function to do that. Don't forget to use ```SceneManager.UnlockLoading()``` to unlock the Loading Process.

Finally, a [SceneTransitionBuilder](/Editor/Build/SceneTransitionBuilder.cs) *Pre Build Processor* was created to check if the **Loading Scene** from all SceneTransition assets has been added to the **Build Settings**. This make sure that you will never waste your time building your game to realize that you forget to add the Loading Scene to the build.

## Installation

### Using the Package Registry Server

Follow the instructions inside [here](https://cutt.ly/ukvj1c8) and the package **ActionCode-Scene Management** 
will be available for you to install using the **Package Manager** windows.

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set. 

- Use the **Package Manager** "Add package from git URL..." feature and paste this URL: `https://github.com/HyagoOliveira/SceneManagement.git`

- You can also manually modify you `Packages/manifest.json` file and add this line inside `dependencies` attribute: 

```json
"com.actioncode.scene-management":"https://github.com/HyagoOliveira/SceneManagement.git"
```
---

**Hyago Oliveira**

[GitHub](https://github.com/HyagoOliveira) -
[BitBucket](https://bitbucket.org/HyagoGow/) -
[LinkedIn](https://www.linkedin.com/in/hyago-oliveira/) -
<hyagogow@gmail.com>