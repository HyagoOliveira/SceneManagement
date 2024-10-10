# Scene Management

* Loads new Scenes using Screen Faders and Loading Scenes.
* Unity minimum version: **2022.3**
* Current version: **3.1.0**
* License: **MIT**
* Dependencies:
    - [com.actioncode.attributes : 2.1.0](https://github.com/HyagoOliveira/Attributes/tree/2.1.0)
    - [com.actioncode.awaitable-coroutines : 1.0.0](https://github.com/HyagoOliveira/AwaitableCoroutines/tree/1.0.0)

## Summary

It's normal for most games to have multiple Scenes. A common user case is to switch between them in a nice way.

This package contains a Scene Manager class which loads new Scenes using customized Screen Faders and Loading Scenes (Scenes created just to show the Loading Progress). 

Also, the current loading scene progress can be shown using a ``TMP_Text``, legacy ```Text```, or ```Slider``` UI components.

![Showcase](/Documentation~/load-scene-showcase.gif "Scene Manager")

## How It Works

In order to use the package, you must first create a **SceneManager** ScriptableObject asset and use it on your scripts.
SceneManager is a ScriptableObject to facilitate how your scripts reference it and avoid to use the Singleton pattern.

To control how to Loading Process should happen, you can also create SceneTransition ScriptableObject assets and change its fields. 

## How To Use

### Creating a Scene Manager ScriptableObject

Open the **Project Settings** Windows and select Scene Manager (below ActionCode group).

Click on the **Create** button and save a new Scene Manager asset.

![The Scene Manager Menu](/Documentation~/scene-manager-menu.png "The Scene Manager Menu")

Click on the Create button next to Default Transition to create and assign a new SceneTransition asset to it.
This transition will be used by the SceneManager every time you don't specify a custom one.

### Scene Transition ScriptableObject

You can manually create Scene Transition ScriptableObject assets by going in Asset > Create > ActionCode > SceneManager > Scene Transition. 

![The Scene Transition](/Documentation~/scene-transition.png "The Scene Transition")

Every field has a describing Tooltip explaining it.

A common user case is to show a Loading Scene when going to different levels in your game and just fades the screen in/out when transit between menus.
This can be done by creating multiples Scene Transition assets and use then to load those parts in your game.

### Using Screen Faders

SceneTransition assets have an attribute for a **Screen Fader Prefab**. This prefab must have a component implementing the [AbstractScreenFader](/Runtime/ScreenFaders/AbstractScreenFader.cs) component.

This package already provides two classes implementing this component:

1. [AnimationScreenFader](/Runtime/ScreenFaders/AnimationScreenFader.cs): fades the screen in and out using a local ```Animation``` component. Perfect to use when fading using animations.
2. [CanvasScreenFader](/Runtime/ScreenFaders/CanvasScreenFader.cs): fades the screen in and out using a local ```CanvasGroup``` component. Use this script and set the attributes to choose how to fade a ```Canvas```.

If those components do not meet your specifications, please feel free to create your own fade component and perhaps contribute to the package. :)

Also, there are two prefabs created using those components at the [Prefabs](/Prefabs/ScreenFaders/) folder. You can use those prefabs or create new prefabs variants using them.

### Loading new Scenes

The example bellow shows the simplest way to load a new scene using the ```SceneManager```:

```csharp
using UnityEngine;
using ActionCode.SceneManagement;

public sealed class LoadingTest : MonoBehaviour
{
    [Scene] public string sceneToLoad;
    public SceneManager sceneManager;    

    public async void Load()
    {
        await sceneManager.LoadSceneAsync(scenesToLoad);
    }
}
```

In this example, we are

1. Using the **Scene** attribute on a ```string``` or ```int``` field to display an Object Field for Scene assets.
2. Creating a reference for **SceneManager** asset and using it to load the ```sceneToLoad``` Scene using ```LoadSceneAsync()``` function. 
This is an asynchronous function so you can hold your code execution at that moment. You can also just use the no asynchronous ```LoadScene()``` function.

### Create Loading Scenes

Inside your Loading Scene, you can use the [LoadingSlider](/Runtime/UI/LoadingSlider.cs) and/or [LoadingText](/Runtime/UI/LoadingText.cs) components to display the current loading progress. Note that in order to use it you must have installed into your project the UI module and TextMeshPro package.

If you want to lock the next scene activation until an action is done, i.e., wait for an input or animation to be completely played inside your Loading Scene, you can use the ```SceneManager.LockLoading()``` function to do that. Don't forget to use ```SceneManager.UnlockLoading()``` to unlock the Loading Process.

### Build Processor

A [SceneTransitionBuilder](/Editor/Build/SceneTransitionBuilder.cs) *Pre Build Processor* was created to check if the **Loading Scene** from all SceneTransition assets has been added to the **Build Settings**. This make sure that you will never waste your time building your game to realize that you forget to add the Loading Scene to the build.

### Scene Loading Events

The SceneManager has **OnLoadingStarted**, **OnLoadingFinished** and **OnProgressChanged** events, invoked at the specific loading time. Use them in your game to execute actions before or after the Scene is loaded.

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