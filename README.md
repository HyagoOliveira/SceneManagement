# Scene Management

* Loads new Scenes using Screen Faders and Loading Scenes
* Unity minimum version: **2019.3**
* Current version: **1.0.0**
* License: **MIT**
* Dependencies:
	- [Animation Parameters 1.0.0](https://bitbucket.org/nostgameteam/animation-parameters/src/1.0.0/)

## Summary

It's normal for most games to have multiple Scenes. A common case is to switch between these scenes in a pleasant way.

This package contains a Scene Loader class which loads new Scenes using customized Screen Faders and Loading Scenes. 

Also, the current loading scene progress can be shown by using a Text or Slider UI component.

![alt text][load-scene-showcase]

## How To Use

### Creating Scene Loading Settings

You can use use any loading methods from [SceneManager](/Scripts/Loading/SceneManager.cs) class. This class is inheriting from the default Unity ```SceneManager``` so you can use any of its methods.

Now, to load Scenes using a Screen Fader or Loading Scenes, you need to create a **Scene Loading Settings** asset. 

Navigate to **Assets > Create > ActionCode > Scene Management > Scene Loading Settings** and create one.

![alt text][scene-loading-settings-inspector]

Fill this asset and use it with any loading functions from ```SceneManager``` class (example bellow). 

### Using Screen Faders

Scene Loading Settings assets have an attribute for a **Screen Fader Prefab**. This prefab must have a component implementing the [AbstractScreenFader](/Scripts/Transitions/AbstractScreenFader.cs) component.

This package provides two components implementing this class:

1. [ScreenFaderAnimator](/Scripts/Transitions/ScreenFaderAnimator.cs): fades the screen in and out using a local ```Animator``` component.
2. [ScreenFaderCanvas](/Scripts/Transitions/ScreenFaderCanvas.cs): fades the screen in and out using a local ```CanvasGroup``` component.

If those components do not meet your specifications, you can create your own component and put your logic there.

Also, there is the [ScreenFaderCanvas](/Prefabs/ScreenFaderCanvas.prefab) prefab with the ```ScreenFaderCanvas``` component applied to it.

![alt text][screen-fader-canvas-prefab-inspector]

You can use this prefab or create a prefab variant and change its ```Image``` color (the fade color) or ```Duration```.

### Loading new Scenes

The example bellow shows how to load new scenes using Scene Loading Settings:

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

1. Use the [Scene](/Scripts/Attributes/SceneAttribute.cs) attribute on a ```string``` or ```int``` field to display an Object Field for Scene assets.
2. Make a reference for your **SceneLoadingSettings** asset and use it to loading new scenes using ```SceneManager.LoadScene``` functions.
3. You can also just fades the screen without loading a new Scene using ```SceneManager.FadeScreen```. You may pass an action to it that will be executed when the screen fades out.

### Create Loading Scenes

Inside your Loading Scene, you can use the [LoadingSlider](/Scripts/Transitions/LoadingSlider.cs) and/or [LoadingText](/Scripts/Transitions/LoadingText.cs) components to display the current loading progress.

If you want to lock the next scene activation until an action is done, i.e., wait for an input or animation to be completely played inside your Loading Scene, you can set the ```SceneManager.LockNextScene``` property and do that.

Finally, a [SceneLoadingBuilder](/Editor/Build/SceneLoadingBuilder.cs) *Pre Build Processor* was created to check if the **Loading Scene** from all SceneLoadingSettings assets has been add to the **Build Settings**.

## Installation

### Using the Package Registry Server

Open the **manifest.json** file inside your Unity project's **Packages** folder and add this code-block before `dependencies` attribute:

```json
"scopedRegistries": [ 
	{ 
		"name": "Action Code", 
		"url": "http://34.83.179.179:4873/", 
		"scopes": [ "com.actioncode" ] 
	} 
],
```

The package **ActionCode-Scene Management** will be available for you to install using the **Package Manager** windows.

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set. 

Use the **Package Manager** "Add package from git URL..." feature or add manually this line inside `dependencies` attribute: 

```json
"com.actioncode.scene-management":"https://bitbucket.org/nostgameteam/scene-management.git"
```

---

**Hyago Oliveira**

[BitBucket](https://bitbucket.org/HyagoGow/) -
[Unity Connect](https://connect.unity.com/u/hyago-oliveira) -
<hyagogow@gmail.com>

[load-scene-showcase]: /Documentation~/load-scene-showcase.gif "Loading Scenes"
[scene-loading-settings-inspector]: /Documentation~/scene-loading-settings-inspector.jpg "Scene Loading Settings"
[screen-fader-canvas-prefab-inspector]: /Documentation~/screen-fader-canvas-prefab-inspector.jpg "Screen Fader Canvas Prefab"
[scene-manager-test-inspector]: /Documentation~/scene-manager-test-inspector.jpg "Scene Manager Test Inspector"