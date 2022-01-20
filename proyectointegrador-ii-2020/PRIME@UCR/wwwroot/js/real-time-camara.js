let imageName = "imagen";

function hasGetUserMedia() {
    let has_um = !!(navigator.mediaDevices &&
        navigator.mediaDevices.getUserMedia);

    if (has_um) {
        console.log('Good to go!');
    } else {
        console.log('Not good to go!');
    }
    return has_um;
}

function openCamera(videoRef) {
    const constraints = {
        video: { width: { exact: 512 }, height: { exact: 384 } }
    };

    if (videoRef != null) {
        navigator.mediaDevices.getUserMedia(constraints).
            then((stream) => { videoRef.srcObject = stream }).catch(handleError);
    }
    else {
        console.error('Null video reference.');
    }

    return true;
}

function takePhotograph(canvasRef, videoRef, imageRef, downloadLinkRef) {
    canvasRef.width = videoRef.videoWidth;
    canvasRef.height = videoRef.videoHeight;
    canvasRef.getContext('2d').drawImage(videoRef, 0, 0);
    imageRef.src = canvasRef.toDataURL('image/webp');
    setImageDowloadLink(imageRef, downloadLinkRef);
    return imageRef.src;
}

function closeCamera(videoRef) {
    console.log('Close Camera');
    if (videoRef.srcObject == null) {
        return true;
    }
    videoRef.srcObject.getTracks().forEach(function (track) {
        track.stop();
    });
    return true;
}

function handleSuccess(stream) {
    video_e.srcObject = stream;
    console.log(video_e.srcObject);
}

function handleError(error) {
    console.error('Error: ', error);
}

function setImageDowloadLink(imageRef, downloadLinkRef) {
    downloadLinkRef.download = imageName;
    downloadLinkRef.href = imageRef.src;
}

function updateImageDownloadName(downloadLinkRef, newName) {
    imageName = newName + ".png";
    downloadLinkRef.download = imageName;
    return true;
}

function clearCanvas(canvasRef) {
    var ctx = canvasRef.getContext('2d');
    ctx.fillStyle = "#000000";
    ctx.fillRect(0, 0, canvasRef.width, canvasRef.height);
    return true;
}