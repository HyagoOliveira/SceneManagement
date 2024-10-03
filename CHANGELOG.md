# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Changed
- Rename function LoadScene -> LoadSceneAsync

### Removed
- UI_MODULE and TEXT_MESH_PRO_MODULE modules

## [2.1.0] - 2024-05-03
### Added
- OnLoadingStarted/Finished events

## [2.0.2] - 2024-04-22
### Fixed
- AnimationScreenFader scale

## [2.0.1] - 2023-03-10
### Changed
- Fix CanvasScreenFader fade in code
- Waits to loading operation is done to fade in

## [2.0.0] - 2022-09-03
### Added
- Awaitable Coroutines package 1.0.0
- Scriptable Settings Provider package 1.0.0
- IScreenFader interface
- AnimationScreenFader component and prefab
- ScreenFaderPool
- SceneTransition
- TextMeshPro support
- Attributes 2.1.0 package dependency
- Lock/Unlock Loading Scene
- LoadScene component

### Changed
- Scriptable Settings Provider for Scene Manager 
- Replace ScreenFaderCanvas -> CanvasScreenFader
- SceneLoadingSettings -> SceneManager

### Removed
- Animation Parameters package
- ScreenFaderAnimator component
- Static SceneManager class
- SceneLoadingSettingsEditor

## [1.0.1] - 2021-01-14
### Changed
- Loading Scenes not added to Build Settings won't stop the build process

## [1.0.0] - 2021-01-13
### Added
- SceneLoadingBuilder builder report
- LoadingText component
- LoadingSlider component
- SceneManager class
- Internal SceneLoader component
- SceneLoadingSettings ScriptableObject
- Animation Parameters package 1.0.0 dependency
- ScreenFaderCanvas Prefab
- ScreenFaderCanvas component
- ScreenFaderAnimator component
- Scene attribute
- CHANGELOG
- README
- Initial commit

[Unreleased]: https://github.com/HyagoOliveira/SceneManagement/compare/2.1.0...main
[2.1.0]: https://github.com/HyagoOliveira/SceneManagement/tree/2.1.0/
[2.0.2]: https://github.com/HyagoOliveira/SceneManagement/tree/2.0.2/
[2.0.1]: https://github.com/HyagoOliveira/SceneManagement/tree/2.0.1/
[2.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/2.0.0/
[1.0.1]: https://github.com/HyagoOliveira/SceneManagement/tree/1.0.1/
[1.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/1.0.0/