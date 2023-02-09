![R2MV](README/Banner.png)

# Episode 11 - Multi-User Training with Normcaore

This example project is part of the [Road to Metaverse, Creator Series](https://create.unity.com/road-to-metaverse). For more information on episode 11, `Masterclass: Build a collaborative multi-user experience`, or if you have questions, check out [this forum thread](https://forum.unity.com/threads/workshops-build-a-collaborative-multi-user-experience.1293408/).

The base for this project is the **VR** scene from the [Visual Scripting XR Interation Toolkit Examples](https://github.com/RoadToTheMetaverse/visualscripting-xri-examples).

Assets & resources used: 
- [Visual Scripting extensions for XR Interaction Toolkit](https://github.com/RoadToTheMetaverse/visualscripring.xrinteractiontoolkit)
- [Visual Scripting Notifications](https://github.com/RoadToTheMetaverse/visualscripting-notifications)
- [Double column car lift](https://grabcad.com/library/double-column-car-lift-1) by Wojciech Gil
- [UI Sfx](https://assetstore.unity.com/packages/audio/sound-fx/ui-sfx-36989) by Little Robot Sound Factory
- [Normcore](https://normcore.io/)

<br>


# What's included
There are **two scenes** in this project: 
- `R2mvImmersiveTraining` main demo scene.
- `WorldInteractionDemo` XR Interaction Toolkit Examples scene.


<br>

# Demo walktrhough

## Preparing project for multi-user
#### Updating State Machine
- Moved from a linear state layout to a “multiple options” layout
- Added a new NetworkStepComplete notification, that will also include the next step index, to transition across all clients from “Any State” to the next step.
Updated all transitions from Any State to handle NetworkStepComplete against their specific index (0, 1, 2, …)

#### Creating a player avatar
- Need a representation of the “other” players
- Can be rudimentary
- Sphere for head
- Controllers for hands
- The local user’s remains the XR Origin in the scene, the network avatar just represents - the user’s head and hands position in the other connected sessions.

## Adding Multi-user capabilities

#### Project setup
- Created a new application Key as described in the [Getting Started](https://normcore.io/documentation/essentials/getting-started.html) section
- Downloaded and installed [asset](https://assetstore.unity.com/packages/tools/network/normcore-free-multiplayer-voice-chat-for-all-platforms-195224#description) from Asset Store

#### Avatar
- Added the Realtime + VR Player prefab to the scene
  - Pasted the App Key
  - Assigned the Local Player components to the Realtime Avatar Manager
  - R2MV VR Player prefab
    - Realtime View - Required to synchornise object over the network
    - Realtime Avatar - Assign head and hands object
    - Realtime Transform - Synchronises position, rotation and scale
  - Head also has the Realtime Voice Avatar and a voice mouth move
- You now have synchronized avatars across multiple sessions and voice chat support!

#### Synchronize Progress
- Normcore uses Data Models to synchronize data across clients
- Data Model
  - Created a new class NormcoreProgressModel
    - Added [RealtimeModel] attribute 
    - Defined a _currenStep int variable and added a [RealtimeProperty()] attribute
  - Back in the Unity Editor
    - Select NormcoreProgressModel in click Compile Model
    - Normcore will generate the boilerplate code for us!
- Created a RealtimeComponent called NormcoreSyncStepComplete that uses NormcoreProgressModel as its data source
  - Listens for local StepCompleted notifications -> updates the Current Step value on the model accordingly
  - Sends a NetworkStepCompleted notification when OncurrentStepDidChange is called.

The Training Manager State Machine handles NetworkStepCompleted, and current step value, to go to a specific section.

- Added both a RealtimeView and NormcoreSyncStepComplete to the TrainingManager (StateMachine)
- Magic!

#### Synchronize Wheels (XR Grab Interactable)
- Added RealtimeView
  - Unchecked Prevent Ownership Takeover
- Added RealtimeTransform
  - Removed Scale Sync
  - Set Rigidbody Settings -> Sleep to Maintaing Ownership While Sleeping to ensure physics are synched after releasing the object
- Created a custom component RequestOwnershipOnGrab
  - Sends an ownership request from the RealtimeView and RealtimeTransform to transfer the ownership to whomever is grabbing that object
  - Not really needed as this could be done in the XR Grab Interactable component Interactable Events

#### Synchronizing Garage Lift
- Similar to progress / step sync
- Uses NormcoreLiftHeightModel and NormcoreSyncLiftHeight
- Updates the Variables variable HeightValue when the HeightDidChange event is called
- When the model value is changed, a network event is called (RPC like)


<br>

---

<br>

## 🚨 Setup

<br>

### VR Required

For this demo we used Oculus Quest via Link Cable to connect to the Editor in Desktop mode.

<br>

---

<br>

## ⚠️ Warning
> This is an **experimental** project **not** officially supported by Unity. Use at your own risk!
