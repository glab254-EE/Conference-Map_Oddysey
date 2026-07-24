let qrScanner = null;

function startScanner() {
    const reader = document.getElementById("reader");
    reader.style.display = "block";
    qrScanner = new Html5Qrcode("reader");
    qrScanner.start(
        {
            facingMode: "environment"
        },
        {
            fps: 10,
            qrbox: 250
        },
        function(decodedText) {
            qrScanner.stop().then(() => {
                reader.style.display = "none";
                unityInstance.SendMessage(
                    "QRScanner",
                    "OnQRCodeDetected",
                    decodedText
                );
            });
        },
        function(errorMessage) {
            // Игнорируем ошибки каждого кадра
        }
    );
}

async function stopScanner() {
    if (!scannerRunning || !qrScanner)
        return;
    try
    {
        await qrScanner.stop();
        await qrScanner.clear();
    } catch (error) {
        console.error("Error stopping QR scanner:", error);
    }
    document.getElementById("reader").style.display = "none";
    qrScanner = null;
    scannerRunning = false;
}