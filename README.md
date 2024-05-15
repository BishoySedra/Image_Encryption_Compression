# Image_Encryption_Compression

Image_Encryption_Compression is a project developed as part of an algorithms academic course.

## Project Overview

This project focuses on implementing image encryption and compression algorithms using LFSR for encryption and Huffman coding for compression. It provides a platform for experimenting with these techniques to enhance image security and reduce file size.

### Problem Definition

#### Image Encryption
Image encryption is the process of encoding an image in such a way that only authorized persons can access it. This is usually done by using a key (i.e., password). Anyone knowing this password can get the original image back, but another password won't work. For example, you could post an encrypted image on the web, but only friends who have the password (and your program) can see the original. Here, a simple algorithm called Linear Feedback Shift Register (LFSR) is used for this purpose.

#### Image Compression
Image compression is a type of data compression applied to digital images to reduce their cost for storage or transmission. One of the common data compression methods is Huffman Coding. Its basic idea is that instead of storing each colour channel as an 8-bit value, it stores the more frequently occurring colour values using fewer bits and less frequently occurring colour values using more bits.

For a detailed description of the project, you can refer to the [Project Description Document](https://cisasuedu.sharepoint.com/:w:/r/sites/ALG24.Term2/Shared%20Documents/General/5%20Project/MATERIALS/%5B1%5D%20Image%20Encryption%20and%20Compression/Image%20encryption%20and%20compression.docx?d=wbce7811032d54c4f855900375f7ed4c4&csf=1&web=1&e=HlR29V).

### Launch Instructions

To successfully launch the project, follow these steps:

1. Open the `.sln` file in your preferred IDE or text editor.

2. Navigate to the `ImageOperations.cs` file and update all string variables with valid paths to ensure the project runs correctly without problems.

3. Press `CTRL + F5` to run the project. This will compile and execute the application.

### Test Cases

For testing the image encryption and compression algorithms, you can download sample test cases and complete test cases from the following links:

- **Sample Test Cases:** [Download Sample Test Cases](https://cisasuedu.sharepoint.com/sites/ALG24.Term2/Shared%20Documents/Forms/AllItems.aspx?csf=1&web=1&e=X6UUx7&cid=fa20e86d%2D0972%2D405f%2Dbaa4%2De9c0112148b9&FolderCTID=0x01200024312EF0CCE30045A4101C13775FBAF2&id=%2Fsites%2FALG24%2ETerm2%2FShared%20Documents%2FGeneral%2F5%20Project%2FMATERIALS%2F%5B1%5D%20Image%20Encryption%20and%20Compression%2FSample%20Test)
- **Complete Test Cases:** [Download Complete Test Cases](https://cisasuedu.sharepoint.com/sites/ALG24.Term2/Shared%20Documents/Forms/AllItems.aspx?csf=1&web=1&e=mkhNcv&cid=f4f75593%2D8065%2D4d7b%2Dad17%2D19e32c33174d&FolderCTID=0x01200024312EF0CCE30045A4101C13775FBAF2&id=%2Fsites%2FALG24%2ETerm2%2FShared%20Documents%2FGeneral%2F5%20Project%2FMATERIALS%2F%5B1%5D%20Image%20Encryption%20and%20Compression%2FComplete%20Test)

### Image Comparison Application

To test the identicality of the two images, you can use the following application:

- **Image Comparison Application:** [Test Image Identicality](https://cisasuedu-my.sharepoint.com/personal/ahmed_salah_cis_asu_edu_eg/_layouts/15/onedrive.aspx?id=%2Fpersonal%2Fahmed%5Fsalah%5Fcis%5Fasu%5Fedu%5Feg%2FDocuments%2FShared%20Resources%2FALG%2F%5BIMAGE%20COMPARATOR%5D&ga=1)

### Open-Source Priority Queue Code

For an open-source implementation of a priority queue, you can refer to the following link:

[Priority Queue Open-Source Code](https://gist.github.com/paralleltree/31045ab26f69b956052c)
