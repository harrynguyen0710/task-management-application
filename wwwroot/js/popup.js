function showPopup(message, callback) {
    document.getElementById('popupMessage').innerText = message;
    document.getElementById('popup').classList.remove('hidden');
    document.getElementById('confirmDelete').onclick = callback;
}

function hidePopup() {
    document.getElementById('popup').classList.add('hidden');
}

function confirmRemoveTask(event) {
    event.preventDefault();
    showPopup("Are you sure you want to delete this task?", function () {
        var form = event.target.closest('form');
        form.submit(); // Send form
    });
}

function confirmRemovePerson(event) {
    event.preventDefault();

    showPopup("Are you sure you want to remove that person?", function () {
        hidePopup();
        var form = event.target.closest('form');
        form.submit(); // Send form
    });
}