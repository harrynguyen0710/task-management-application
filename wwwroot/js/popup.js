function showPopup(message, callback) {
    document.getElementById('popupMessage').innerText = message;
    document.getElementById('popup').classList.remove('hidden');
    document.getElementById('confirmDelete').onclick = callback;
}

function hidePopup() {
    document.getElementById('popup').classList.add('hidden');
}

function showPopupNotification(message) {
    var popup = document.getElementById('popupNotification');
    var messageElement = document.getElementById('popupMessage');
    messageElement.textContent = message;
    popup.classList.remove('hidden');

    setTimeout(function () {
        popup.classList.add('hidden');
    }, 3000);
}

function confirmRemoveTask(event) {
    event.preventDefault();
    showPopup("Are you sure you want to delete this task?", function () {
        hidePopup();
        var form = event.target.closest('form');
        form.submit(); // Send form
        showPopupNotification('Task removed successfully!');
    });
}

function confirmRemovePerson(event) {
    event.preventDefault();
    showPopup("Are you sure you want to remove that person?", function () {
        hidePopup();
        var form = event.target.closest('form');
        form.submit();
        showPopupNotification('User removed successfully!');
    });
}

function openModal(name, startDate, status, priority, assignee, dueDate, description) {
    document.getElementById('taskName').innerText = name;
    document.getElementById('taskStartDate').innerText = startDate;
    document.getElementById('taskStatus').innerText = status;
    document.getElementById('taskPriority').innerText = priority;
    document.getElementById('taskAssignee').innerText = assignee;
    document.getElementById('taskEndDate').innerText = dueDate;
    document.getElementById('taskDescription').innerText = description;
    const taskPriority = document.getElementById('taskPriority');
    const taskStatus = document.getElementById('taskStatus');

    taskPriority.classList.remove('bg-red-500', 'bg-yellow-500', 'bg-green-500', 'bg-gray-500', 'text-white', 'text-black');
    switch (priority) {
        case 'High':
            taskPriority.classList.add('bg-red-500', 'text-white');
            break;
        case 'Medium':
            taskPriority.classList.add('bg-yellow-500', 'text-black');
            break;
        case 'Low':
            taskPriority.classList.add('bg-green-500', 'text-white');
            break;
        default:
            taskPriority.classList.add('bg-gray-500', 'text-white');
            break;
    }

    taskStatus.classList.remove('bg-red-500', 'bg-yellow-500', 'bg-green-500', 'bg-gray-500', 'text-white', 'text-black');
    switch (status) {
        case 'Due':
            taskStatus.classList.add('bg-red-500', 'text-white');
            break;
        case 'Completed':
            taskStatus.classList.add('bg-yellow-500', 'text-black');
            break;
        case 'In Progress':
            taskStatus.classList.add('bg-green-500', 'text-white');
            break;
        default:
            taskStatus.classList.add('bg-gray-500', 'text-white');
            break;
    }

    //Show modal
    document.getElementById('taskModal').classList.remove('hidden');
}

// Close modal
function closeModal(event) {
    if (event) {
        event.stopPropagation();
    }
    document.getElementById('taskModal').classList.add('hidden');
}
