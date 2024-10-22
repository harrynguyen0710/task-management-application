function loadTasks(pageNumber, projectId) {
    console.log('Loading tasks for page ' + pageNumber);
    $.ajax({
        url: '/Project/LoadTasks', // The action method for loading tasks
        type: 'GET',
        data: { projectId: projectId, pageNumber: pageNumber },
        success: function (result) {
            $('#taskSection').html(result);
        },
        error: function (error) {
            console.error('Error loading tasks:', error);
        }
    });
}