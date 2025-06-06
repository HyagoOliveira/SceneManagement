# Scene Management

* Loads new Scenes using Screen Faders and Loading Scenes.
* Unity minimum version: **2022.3**
* Current version: **4.3.0**
* License: **MIT**
* Dependencies:
    - [com.actioncode.attributes : 2.1.0](https://github.com/HyagoOliveira/Attributes/tree/2.1.0)
    - [com.actioncode.awaitable-coroutines : 1.0.0](https://github.com/HyagoOliveira/AwaitableCoroutines/tree/1.0.0)

## Summary

It's normal for most games to have multiple Scenes. A common user case is to switch between them in a nice way.

This package contains a Scene Manager class which loads new Scenes using customized Screen Faders and Loading Scenes (Scenes created just to show the Loading Progress). 

Also, the current loading scene progress can be shown using a ``TMP_Text``, legacy ```Text```, or ```Slider``` UI components. You may have TextMesh Pro, UI or both packages installed into your project and choose which component to use.

![Showcase](/Documentation~/load-scene-showcase.gif "Scene Manager")

## How It Works

You can load Scenes using the static class [SceneManager](/Runtime/SceneManager/SceneManager.cs) or using [LoadScene](/Runtime/Utils/LoadScene.cs) component.

To control how the Loading Process should happen, you can create a SceneTransition ScriptableObject asset and change its fields. 

## How To Use

### Scene Transition ScriptableObject

You can manually create Scene Transition ScriptableObject assets by going in Asset > Create > ActionCode > SceneManager > Scene Transition. 

![The Scene Transition](/Documentation~/scene-transition.png "The Scene Transition")

Every field has a describing Tooltip explaining it.

A common user case is to show a Loading Scene when going to different levels in your game and just fades the screen in/out when transit between menus.
This can be done by creating multiples Scene Transition assets and use then to load those parts in your game.

You can also use the [DefaultSceneTransition](/Transitions/DefaultSceneTransition.asset) provided in this package. This one will not use a Loading Scene, just a screen fader.

### Using Screen Faders

SceneTransition assets have an attribute for a **Screen Fader Prefab**. This prefab must have a component implementing the [AbstractScreenFader](/Runtime/ScreenFaders/AbstractScreenFader.cs) component.

This package already provides two components implementing this component:

1. [AnimationScreenFader](/Runtime/ScreenFaders/AnimationScreenFader.cs): fades the screen in and out using a local ```Animation``` component. Perfect to use when fading using animations.
2. [CanvasScreenFader](/Runtime/ScreenFaders/CanvasScreenFader.cs): fades the screen in and out using a local ```CanvasGroup``` component. Use this script and set the attributes to choose how to fade a ```Canvas```.

If those components do not meet your specifications, please feel free to create your own fade component and perhaps contribute to the package. :)

Also, there are two prefabs created using those components at the [Prefabs](/Prefabs/ScreenFaders/) folder. You can use those prefabs or create new variants of them.

### Loading new Scenes by Code

You just need to use any of the loading function provided by the static class [SceneManager](/Runtime/SceneManager/SceneManager.cs).

```csharp
using UnityEngine;
using ActionCode.SceneManagement;

public sealed class LoadingTest : MonoBehaviour
{
    [Scene] public string sceneToLoad;
    public Scene otherScene;
    public SceneTransition sceneTransition;

    public async void LoadAsync()
    {
        // do something here
        await SceneManager.LoadSceneAsync(sceneToLoad, sceneTransition);
        // do other things here after scene is loaded
    }

    public void LoadOther() => SceneManager.LoadScene(otherScene, sceneTransition);   
}
```

In this example, we are

1. Using the **Scene** attribute on a ```string``` or ```int``` field to display an Object Field to find a Scene asset.
2. You can also use **Scene** class to display an Object Field to find a Scene asset.
3. Using the asynchronous function ```LoadSceneAsync``` to load a Scene asynchronously and hold your code execution at that moment.
4. Using the function ```LoadScene``` to load a Scene synchronously.

Both load functions receive a ```SceneTransition``` parameter that can be null.

### Create Loading Scenes

Inside your Loading Scene, you can use the [LoadingSlider](/Runtime/UI/LoadingSlider.cs) and/or [LoadingText](/Runtime/UI/LoadingText.cs) components to display the current loading progress. Note that in order to use it you must have installed into your project the UI module and/or TextMeshPro package.

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