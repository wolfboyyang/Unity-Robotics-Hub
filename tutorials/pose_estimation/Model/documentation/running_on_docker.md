Docker 
======

Another option to run the project is to use a Docker image. This option allows you to avoid downloading the project's libraries to your local computer, while still running the project successfully. With a Docker image, you also have the ability to train or evaluate your model on a cloud platform, such as Google Cloud Platform, AWS, Microsoft Cloud, and many others. 

## Docker Requirements
You will need to have [Docker](https://docs.docker.com/get-docker/) installed on your computer. 

### Docker Creation on Local

* **Action**: In [config.yaml](../config.yaml), under `system`, set the argument `log_dir_system` to: `/save/single_cube`. 
* **Action**: Set the argument `data_root` under `system` to `/data`. 

Before creating the Docker image, you need to be sure your Docker settings are compatible with the project. Open Docker Desktop, click on `Settings` (the gear icon) on the top right, and go to `Resources`. Then change your settings so that it matches the following: 

<p align="center">
<img src="docs/docker_settings.png"/>
</p>

**Note**: You may need to tweak these settings for your exact use case.

The first step is to build the Docker image.

* **Action**: Open a new terminal and put yourself in the `Pose-Estimation-Model` folder. Then enter the following:
```bash 
docker build -t [IMAGE NAME] .
```

As an example, for the image name, I used `pose_estimation`, and thus my command is: 
```bash 
docker build -t pose_estimation .
```

**Note**: If you change something in the project, you will need to rebuild the Docker image. 

* **Action**: Now we need to run the Docker image. One way is to use the bash shell. Still in the same terminal, enter the following:
```bash
docker run -it -v [FULL PATH TO DATA FOLDER]:/data -v [FULL PATH TO MODEL FOLDER]:/save/single_cube [IMAGE NAME] bash
```

The `FULL PATH TO DATA FOLDER` is the path to the upper directory of your data. As an example, I have put my `UR3_single_cube_training` and `UR3_single_cube_validation` data folder into a folder called `data` that I have created in my `Documents` folder. Thus my `FULL PATH TO DATA FOLDER` will be `/Users/jonathan.leban/Documents/data`.

The `FULL PATH TO MODEL FOLDER` is the directory in which your models and metrics will be saved. For me, I created a folder called `save` into my Documents. 
The `/save/single_cube` directory is the directory inside the docker container. That is why in the [config.yaml](../config.yaml) file, under the argument `system` the argument `log_dir_system` is set to `/save/single_cube`. 

Thus, the final command for me is: 
```bash
docker run -it -v /Users/jonathan.leban/Documents/data:/data -v Users/jonathan.leban/Documents/save:/save/single_cube pose_estimation bash
```

### Commands Model
#### Train
To run the training command, you need to adopt the following format: 

```bash
cli.py train [options] [config] [dataset] [training-options] [hyperparameter-options] [save-options] [loading-options]
```

Thus, if you want to keep the [config.yaml](../config.yaml) as it is and change the number of epochs to 5, the training batch 
size to 10 and set to 20 the number of steps you need to accumulate to upgrade the gradient, then the command will be: 
* **Action**: 
```bash 
python -m pose_estimation.cli train --epochs=5 --batch-training-size=10 --accumulation-steps=20
```

#### Evaluate  
To run the evaluate command, you need to adopt the following format: 

```bash
cli.py evaluate [options] [config] [dataset] [evaluation-options] [save-options] [loading-options]
```

Thus, if you want to keep the [config.yaml](../config.yaml) as it is and change the test batch size to 10 and the path where you have saved the already trained model you want to use which is for me `/Users/jonathan.leban/Documents/save/UR3_single_cube_model_ep120.tar`. 
* **Action**: 
```bash 
python -m pose_estimation.cli evaluate --batch-test-size=10 --load-dir-checkpoint=/Users/jonathan.leban/Documents/save/UR3_single_cube_model_ep120.tar
```

### Copy metrics and models saved on Docker on your local machine 
Once you have trained or evaluated your model, you may want to copy the results out of the docker container, to your local computer. 

After building and running the docker image your terminal should look something like this:

<p align="center">
<img src="docs/docker_id_image.png" height=50/>
</p>

Here you can see on the right of `root@` the id of the docker container you are in. Copy this id. 

As a reminder, we want to extract some files of `save/single_cube/` inside the docker container into your `save` folder you have created on your local computer. 
Open a new terminal and enter the following: 

```bash
docker cp <containerId>:/file/path/within/container /host/path/target
```

As an example, I have put my `save` folder inside my `/Users/jonathan.leban/Documents/`. Thus, to copy my model on my local, I will enter the following: 
```bash
docker cp 48a81368b095:/save/single_cube/UR3_single_cube_model_ep120.tar /Users/jonathan.leban/Documents/save
```

To copy my metrics on my local, I will enter the following: 
```bash
docker cp 48a81368b095:/save/single_cube/events.out.tfevents.1612402202.48a81368b095 /Users/jonathan.leban/Documents/save
```

The metrics folder should have the same format as **events.out.tfevents.<`number`>.<`number`>**

### Troubleshooting 
If when you launch the training you have an issue saying `Killed`, then you need to increase your `Memory` to 8GB in your Docker settings. 