# Bachelor thesis project "Interactions between Virtual Agents and Smart Objects in Mixed Reality Learning Applications"
The developed application is called ARTutor. It offers a learning experience for acquiring the Estonian language in augmented reality (AR). Upon opening the app, the learner is invited to place virtual objects and label physical objects in their physical environment. Then, a virtual avatar called Mira offers to demonstrate the learner four different interactions on each placed or labeled object. Before and after each interaction, in-app flash cards with the expressions matching Mira’s interactions are presented to the learner. The flash cards are in Estonian with English translations.


# Installation

The Unity project is compatible with Unity 2020.3.27f1+. The Xcode build is compatible with Xcode Version 13.4.1+. ARTutor was developed for the 12.9-inch iPad Pro 2021 with LiDAR.

### Working with the Unity project

1. Install Unity Hub and download Unity 2020.3.27f1.
1. Open the project in Unity and open the `Meshing` scene from the project files.
1. Install the [Remote Plugin](https://assetstore.unity.com/packages/tools/utilities/ar-foundation-remote-2-0-201106) for Unity or replace the scripts MeshClassificationFracking and MeshClassificationManager with theor original versions from [arfoundation-demos](https://github.com/Unity-Technologies/arfoundation-demos).

### Creating an iOS build

A ready-made build is included on the `release` branch of the project. Unless you would like to create your own build, you can skip this part of the instruction.

1. If you installed the [Remote Plugin](https://assetstore.unity.com/packages/tools/utilities/ar-foundation-remote-2-0-201106), set it up according to the instructions provided by its developer.
1. In the project settings, select ARKit and ARCode as plug-in providers for iOS and Android respectively under XR Plug-in Management.
1. In the project settings, change the Player settings to match your Xcode company name, bundle identifier, etc. Besides, add the mandatory Camera Usage Description and Microphone Usage Description. Finally, set the Target minimum iOS Version to 11.0.
1. In the build settings of the project, select the Meshing scene, choose iOS as the platform, select Xcode Version 13.4.1 or later, and check Development build and Deep profiling support.
1. Build the project.

### Running an iOS build

1. Open the project in Xcode and adjust your General and Signing & Capabilities settings to match your bundle identifier, team, etc. Besides, select the Automatically manage signing option.
1. Connect an iPad and select is as a target device in Xcode.
1. Run the build.

If you have any questions regarding the installation process, contact me via [danylo.bekhter@rwth-aachen.de](mailto:danylo.bekhter@rwth-aachen.de).

# Used repositories and resources

1. [Virtual Agents Framework](https://github.com/rwth-acis/Virtual-Agents-Framework)
1. [ARFoundation Demos](https://github.com/Unity-Technologies/arfoundation-demos)
1. [Painless Inventory](http://toqoz.fyi/unity-painless-inventory.html)

<!---

# Functions from  the arfoundation-demos demo projects

The following functions embedded in ARTutor rely on [*AR Foundation 4.1.7*](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/manual/index.html). These functions were adopted from Unity's and Apple's demo projects showcasing more advanced functionalities from their latest toolkits.

This set of demos relies on four Unity packages:

* ARSubsystems ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@4.1/manual/index.html))
* ARCore XR Plugin ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arcore@4.1/manual/index.html))
* ARKit XR Plugin ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arkit@4.1/manual/index.html))
* ARFoundation ([documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/manual/index.html))

ARSubsystems defines an interface, and the platform-specific implementations are in the ARCore and ARKit packages. ARFoundation turns the AR data provided by ARSubsystems into Unity `GameObject`s and `MonoBehavour`s.


### Building for Unity 2020.2
When building for *Android in Unity 2020.2* you need to modify the following settings under Project Settings / Player / Publishing Settings
* Uncheck Custom Main Gradle Template and 
* Uncheck Custom Launcher Gradle Template  

These are been removed during the upgrade to Unity 2020.3 LTS

  
#[Image Tracking](#image-tracking--also-available-on-the-asset-store-here) | [Onboarding UX](#ux--also-available-on-the-asset-store-here) | [Mesh Placement](#mesh-placement) | [Shaders](#shaders)
#------------ | ------------- | ------------- | ----------------

  
# Mesh Placement
![img](https://user-images.githubusercontent.com/2120584/87866691-77e47080-c939-11ea-9fe9-25a68ddd8a4b.JPG)
An example scene for using [ARKit meshing](https://docs.unity3d.com/Packages/com.unity.xr.arkit@4.0/manual/arkit-meshing.html) feature with the available surface [classifications](https://developer.apple.com/documentation/arkit/armeshclassification) to place unique objects on surfaces. This demo adds some additional functionality for use cases helpful outside of this demo such as a placement reticle and the [DOTween tweening library](http://dotween.demigiant.com/). 

## Disclaimer
Meshing is only supported through ARKit on LiDAR Enabled devices (iPad Pro, iPhone 12 Pro, iPhone 13 Pro)

## Mesh Classificatons
Classifying the surfaces is managed by the [MeshClassificationManager.cs](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/MeshClassificationManager.cs) which maintains a [Dictionary](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/MeshClassificationManager.cs#L49) of TrackableID's and a native array of ARMeshClassifications. By subscribing to the meshesChanged event on the AR Mesh Manager we maintain the dictionary of added, updated and removed meshes based on trackable ID's of the meshes generated. 

> there is currently an issue with the trackable ID's on the meshes found so we use the string name is order to properly extract and store the correct trackable ID of the meshes

Once we have an up to date dictionary we can query it based on a trackable ID as the key and a triangle index as the index in our native array. This returns an ARMeshClassification enum. 

To update the label at the top of the demo we use a physics raycast to raycast against the megamesh generated by the ARMeshManager to get the correct triangle index and parse the current classification for a more readable string label. 
> to generate a mesh collider for physics raycast our megamesh must contain a mesh collider component on it

## Mesh Placement
The [Mesh Placement Manager](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/ClassificationPlacementManager.cs) script handles showing the UI for each unique surface and spawning the objects at the placement reticle position. In the Update method I am checking against [specific classifications](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/ClassificationPlacementManager.cs#L101-L104), in this case Table, Floor and Wall to enable or disable specific UI buttons. The UI buttons are configured in the scene to pass an index and instantiate the assigned prefab in the object list for each surface.

There's also some additional logic for placing floor and table objects to rotate them towards the user (Camera transform).

## Placement Reticle
A way to place content on surfaces based on the center screen position of the users device. This reticle shows a visual that can snap to mesh (generated ARKit mesh) or planes. It uses an AR raycast to find the surfaces and snaps to AR Raycast Hit pose position and rotation. 

There is also additional logic to scale up the reticle's local scale based on the distance away from the user (AR Camera transform). 

For determining between snapping to a mesh and a plane we use a Raycast Mask. 

Mesh:
```m_RaycastMask = TrackableType.PlaneEstimated;```
Plane:
```m_RaycastMask = TrackableType.PlaneWithinPolygon;```


## Mesh Key
To visualize and understand the different classified surfaces we are using a modified version of the [MeshFracking](https://github.com/Unity-Technologies/arfoundation-samples/blob/main/Assets/Scenes/Meshing/Scripts/MeshClassificationFracking.cs) script available in AR Foundaiton Samples. We've added an additional helper method to modify the alpha color of the generated meshes [ToggleVisability().](https://github.com/Unity-Technologies/arfoundation-demos/blob/master/Assets/Meshing/Scripts/MeshClassificationFracking.cs#L350-L366) This is all driven by a Toggle UI button in the scene and changes the shared material color on each material on the generated prefabs. By default they are configured to be completely transparent.


## DOTween is available on the Unity Asset store [here](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
For this demo it is used to scaling up the placed objects as they appear. 

It was developed by Daniele Giardini - Demigiant and is Copyright (c) 2014. Full License for DOTween available [here](http://dotween.demigiant.com/license.php)

-->