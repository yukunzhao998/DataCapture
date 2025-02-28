# Data Capture App

**Explanation and usage for the application designed for collecting data based on Unity.** 

## Getting Started
#### Prerequisites
1. Clone this project
```git clone https://github.com/yukunzhao998/DataCapture.git```
2. Install Unity with Android build support / IOS build support (editor version at least 2021.3.8f1 or higher)
3. Install Xcode if built for IOS.

#### Installation
Open the ```DataCapture``` project in Unity.
* IOS
    >1. File -> Build Settings -> Platform switch to IOS -> Build And Run
    >2. Open ```Unity-iPhone.xcodeproj``` in Xcode -> Product -> Destination -> IOS Device ->Signing & capacilities -> Run
* Android
    > 1. File -> Build Settings -> Platform switch to Android -> Build
    > 2. Install apk file to device

#### Exporting data
* IOS
    > Open Xcode -> Window -> Device and Simulators -> Select DataCapture ->Download Container
* Android
    > The data is stored in ```Internal storage/Android/data/com.DefaultCompany.DataCapture``` by default.

## Usage and data format
When launching for the first time, allow camera permissions and location permissions. Press ```Start``` button to start a new sequence of capturing and ```Stop``` button to stop the sequence of capturing. The Red text shows the number of images that have been captured in this sequence.

You can adjust fixed timestamp with the input field when capturing, the input float means the interval time (in second) between two continuous frames, which is 1 by default. For example, if you enter 0.5, the program will capture at 2 fps. You can also set the interval time of capturing in ```Build Settings -> Player Settings -> Time -> Fixed Timestamp```. The images and data files described below are stored locally in the device.

We provide txt data files, which include VIO, gyroscope, GPS, accelerometer and magnetometer data respectively. 

* VIO data

  For ARKitPose/ARCorePose.txt poses files:

  ```image_name x y z qw qx qy qz```.

  ```x, y, z``` are coordinates of position,  6DoF poses follow the Unity coordinate system.

* Gyroscope data

  For GyroscopeData.txt files: 

  ```image_name qw qx qy qz```

  The quaternion follows the Unity coordinate system and represents the device's orientation. 

* GPS data

  For GPSData.txt files:

  ```image_name x y z```

  ```x, y, z``` are latitude, longitude and altitude respectively.

* Accelerometer data

  For AccelerometerData.txt files: 

  ```image_name x y z```

  ```x y z``` are last measured linear acceleration of a device in three-dimensional space following the Unity coordinate system.

* Magnetometer data

  For MagnetometerData.txt files:

  ```image_name trueHeading```

  ```trueHeading``` is the heading in degrees relative to the geographic North Pole.

## Acknowledgments
#### Reference
Our Data Capture app is used in [Paper](https://ieeexplore.ieee.org/document/10536574) if you find this repo helpful, please cite:
<h3 id="citation">Citation</h3>
  <pre class="citation-code"><code><span>@inproceedings{liu2024marvin,
  title={MARViN: Mobile AR Dataset with Visual-Inertial Data},
  author={Liu, Changkun and Zhao, Yukun and Braud, Tristan},
  booktitle={2024 IEEE Conference on Virtual Reality and 3D User Interfaces Abstracts and Workshops (VRW)},
  pages={532--538},
  year={2024},
  organization={IEEE}
}}</code></pre>

#### MARViN: Mobile AR Dataset with Visual-Inertial Data

##### [Homepage](https://lck666666.github.io/research/MARViN/index.html) | [Paper](https://ieeexplore.ieee.org/document/10536574)

**MARViN: Mobile AR Dataset with Visual-Inertial Data** <br>
[Changkun Liu](https://lck666666.github.io)\*, [Yukun Zhao](https://scholar.google.com/citations?view_op=list_works&hl=zh-CN&user=NcLael4AAAAJ)\* and [Tristan Braud](https://braudt.people.ust.hk/index.html) <br>
<em>* equal contribution</em><br>
The Hong Kong University of Science and Technology<br>
Virtual Reality and 3D User Interfaces Abstracts and Workshops (VRW), 2024<br>
