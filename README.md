# Scene Management

* Loads new Scenes using Screen Faders and Loading Scenes.
* Unity minimum version: **2019.3**
* Current version: **1.0.1**
* License: **MIT**
* Dependencies:
    - [com.actioncode.scriptable-settings-provider : 1.0.0](https://github.com/HyagoOliveira/ScriptableSettingsProvider/tree/1.0.0)

## Summary

It's normal for most games to have multiple Scenes. A common user case is to switch between them in a nice way.

This package contains a Scene Loader class which loads new Scenes using customized Screen Faders and Loading Scenes (Scenes created just to show loading progress). 

Also, the current loading scene progress can be shown using a ```Text``` or ```Slider``` UI component.

![alt text][load-scene-showcase]

## How To Use

### Creating a Scene Loading Settings

You can use any loading methods from the static [SceneManager](/Runtime/Loading/SceneManager.cs) class. This class is inheriting from the default Unity ```SceneManager``` so you can use any of its methods as well.

Now, to load Scenes using this new class you'll need to create a **Scene Loading Settings** asset (a ```ScriptableObject```).

Navigate to **Assets > Create > ActionCode > Scene Management > Scene Loading Settings** and create one.

![alt text][scene-loading-settings-inspector]

Fill this asset and use it with any loading functions from ```SceneManager``` class (available example bellow). 

### Using Screen Faders

Scene Loading Settings assets have an attribute for a **Screen Fader Prefab**. This prefab must have a component implementing the [AbstractScreenFader](/Runtime/Transitions/AbstractScreenFader.cs) component.

This package provides two components implementing this component:

1. [ScreenFaderAnimator](/Runtime/Transitions/ScreenFaderAnimator.cs): fades the screen in and out using a local ```Animator``` component. Perfect to use when fading using animations.
2. [ScreenFaderCanvas](/Runtime/Transitions/ScreenFaderCanvas.cs): fades the screen in and out using a local ```CanvasGroup``` component. Use this script and set the attributes to choose how to fade a ```Canvas```.

If those components do not meet your specifications, please feel free to create your own fade component and contribute to the package. :)

Also, there is the [ScreenFaderCanvas](/Prefabs/ScreenFaderCanvas.prefab) prefab with the ```ScreenFaderCanvas``` component already applied to it.

![alt text][screen-fader-canvas-prefab-inspector]

You can use this prefab or create a prefab variant and change its ```Image``` color (the fade color) or ```Duration```.

### Loading new Scenes

The example bellow shows how to load new scenes using Scene Loading Settings asset:

```csharp
using UnityEngine;
using UnityEngine.UI;
using ActionCode.SceneManagement;

public sealed class SceneManagerTest : MonoBehaviour
{
    [Scene] public string sceneToLoad = "SceneB";
    public SceneLoadingSettings settings;

    public Image panelImage;
    public Color panelColor;

    public void LoadScene() => SceneManager.LoadScene(sceneToLoad, settings);

    public void GoPreviousScene() => SceneManager.LoadPreviousScene(settings);

    public void GoNextScene() => SceneManager.LoadNextScene(settings);

    public void FadeScreen() => SceneManager.FadeScreen(settings, SwapPanelColor);

    public void SwapPanelColor() => panelImage.color = panelColor;
}
```

![alt text][scene-manager-test-inspector]

In this example, you can:

1. Use the [Scene](/Runtime/Attributes/SceneAttribute.cs) attribute on a ```string``` or ```int``` field to display an Object Field for Scene assets.
2. Make a reference for your **SceneLoadingSettings** asset and use it to loading new scenes using ```SceneManager.LoadScene()``` functions.
3. You can also just fades the screen without loading a new Scene using ```SceneManager.FadeScreen```. You may pass an action to it and it'll be executed when the screen fades out.

### Create Loading Scenes

Inside your Loading Scene, you can use the [LoadingSlider](/Runtime/Transitions/LoadingSlider.cs) and/or [LoadingText](/Runtime/Transitions/LoadingText.cs) components to display the current loading progress.

If you want to lock the next scene activation until an action is done, i.e., wait for an input or animation to be completely played inside your Loading Scene, you can set the ```SceneManager.LockNextScene``` property and do that.

Finally, a [SceneLoadingBuilder](/Editor/Build/SceneLoadingBuilder.cs) *Pre Build Processor* was created to check if the **Loading Scene** from all SceneLoadingSettings assets has been add to the **Build Settings**. This make sure that you will never waste your time building your game to realize that you forget to add a particular Scene to the build.

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

[load-scene-showcase]: /Documentation~/load-scene-showcase.gif "Loading Scenes"
[scene-loading-settings-inspector]: /Documentation~/scene-loading-settings-inspector.jpg "Scene Loading Settings"
[screen-fader-canvas-prefab-inspector]: /Documentation~/screen-fader-canvas-prefab-inspector.jpg "Screen Fader Canvas Prefab"
[scene-manager-test-inspector]: /Documentation~/scene-manager-test-inspector.jpg "Scene Manager Test Inspector"