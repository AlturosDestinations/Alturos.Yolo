# Alturos.ImageAnnotation

The purpose of this project is to manage training data for Neural Networks. The data is stored at a central location, for example Amazon S3.
In our case we have image data for different runs that we want to annotate together. Every run is stored in an AnnotationPackage.

For every AnnotationPackage we have some Metadata, Weather, Color, ... this informations we are store in a DynamoDB Table.

![object detection result](/doc/AlturosImageAnnotation.png)

## Requirements

 - [Amazon AWS Account](https://aws.amazon.com/) (S3, DynamoDB)
 
## Features

 - Image annotation together
 - Verify Image annotation data
 - Export for yolo (train.txt, test.txt, obj.names) with filters
