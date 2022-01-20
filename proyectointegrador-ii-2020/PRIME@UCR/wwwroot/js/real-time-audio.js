let timerRef;
let decsTimerInterval;
let secTimerInterval;
let minTimerInterval;
let decsCounter = 0;
let secCounter = 0;
let minCounter = 0;

let hiddenClass = "hidden";
let downloadLinkRef;
let audioName = "audio";


function initAudio(record, stop, audio, _timerRef, downloadLink, fillDiv) {
    timerRef = _timerRef;
    downloadLinkRef = downloadLink;
    if (navigator.mediaDevices.getUserMedia) {
        console.log('getUserMedia supported.');

        const constraints = { audio: true };
        let chunks = [];

        let onSuccess = function (stream) {
            const mediaRecorder = new MediaRecorder(stream);

            record.onclick = function () {
                audio.classList = "hidden";
                fillDiv.classList = "";

                initTimer();

                mediaRecorder.start();
                console.log(mediaRecorder.state);
                console.log("recorder started");

                record.classList.add(hiddenClass);
                stop.classList.remove(hiddenClass);
            }

            stop.onclick = function () {
                audio.classList = "";
                fillDiv.classList = "hidden";

                stopTimer();

                mediaRecorder.stop();
                console.log(mediaRecorder.state);
                console.log("recorder stopped");

                record.classList.remove(hiddenClass);
                stop.classList.add(hiddenClass);
            }

            mediaRecorder.onstop = function (e) {
                console.log("data available after MediaRecorder.stop() called.");

                //const clipName = 'audio';

                audio.setAttribute('controls', '');

                audio.controls = true;
                const blob = new Blob(chunks, { 'type': 'audio/ogg; codecs=opus' });
                chunks = [];
                const audioURL = window.URL.createObjectURL(blob);
                audio.src = audioURL;
                console.log(audio);
                console.log(audio.src);
                console.log("recorder stopped");

                downloadLink.href = audio.src;
                downloadLink.download = audioName;

            }

            mediaRecorder.ondataavailable = function (e) {
                chunks.push(e.data);
            }
        }

        let onError = function (err) {
            console.log('The following error occured: ' + err);
        }

        navigator.mediaDevices.getUserMedia(constraints).then(onSuccess, onError);
        return true;

    } else {
        console.log('getUserMedia not supported on your browser!');
        return false;
    }
}

function initTimer() {
    decsTimerInterval = setInterval(descCounterFunc, 100);
    secTimerInterval = setInterval(secCounterFunc, 1000);
    minTimerInterval = setInterval(minCounterFunc, 60000);
}

function descCounterFunc() {
    decsCounter++;
    updateTimer();
}

function secCounterFunc() {
    secCounter++;
    decsCounter = 0;
    updateTimer();
}

function minCounterFunc() {
    minCounter++;
    secCounter = 0;
    updateTimer();
}

function updateTimer() {
    var minText = minCounter.toString();
    if (minCounter < 10) minText = '0' + minText;
    var secText = secCounter.toString();
    if (secCounter < 10) secText = '0' + secText;
    var decsText = decsCounter.toString();
    if (decsCounter < 10) decsText = '0' + decsText;

    timerRef.innerText = minText + ':' + secText + ':' + decsText;
}

function stopTimer() {
    clearInterval(decsTimerInterval);
    clearInterval(secTimerInterval);
    clearInterval(minTimerInterval);
    decsCounter = secCounter = minCounter = 0;
    //timerRef.innerText = '';
}

function updateAudioName(newName) {
    audioName = newName;
    downloadLinkRef.download = audioName;
    return true;
}