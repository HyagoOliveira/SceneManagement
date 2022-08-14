# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- Scriptable Settings Provider package 1.0.0
- AnimationScreenFader component and prefab

### Changed
- Scene Loading Setting asset uses Scriptable Settings Provider
- Replace ScreenFaderCanvas -> CanvasScreenFader

### Removed
- Animation Parameters package
- ScreenFaderAnimator component
- Static SceneManager class

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

[Unreleased]: https://github.com/HyagoOliveira/SceneManagement/compare/1.0.1...main
[1.0.1]: https://github.com/HyagoOliveira/SceneManagement/tree/1.0.1/
[1.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/1.0.0/