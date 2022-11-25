function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteVar = 'delete_' + uniqueId;
    var confirmDelete = 'confirmDelete_' + uniqueId;
    var cancelDelete = 'cancelDelete_' + uniqueId;

    if (isDeleteClicked) {
        document.getElementById(deleteVar).style.display = "none";
        document.getElementById(confirmDelete).style.display = "inline-block";
        document.getElementById(cancelDelete).style.display = "inline-block";
    } else {
        document.getElementById(deleteVar).style.display = "inline-block";
        document.getElementById(confirmDelete).style.display = "none";
        document.getElementById(cancelDelete).style.display = "none";
    }
}


function changeInfo(isClicked) {
    var userInfoFormId = 'infoForm';
    var orderHistoryFormId = 'orderHistoryForm';
    var commentForm = 'commentForm';
    if (isClicked == 'showOrders') {
        document.getElementById(userInfoFormId).style.display = 'none';
        document.getElementById(orderHistoryFormId).style.display = 'inline-block';
        document.getElementById(commentForm).style.display = 'none';
        
    } else if (isClicked == 'showUsers') {
        document.getElementById(userInfoFormId).style.display = 'inline-block';
        document.getElementById(orderHistoryFormId).style.display = 'none';
        document.getElementById(commentForm).style.display = 'none';
        document.getElementById("infoForm").style.animation = 'none'
        document.getElementById("infoH1").style.animation = 'none'
        document.getElementById("userRoundButton").style.animation = 'none'
        document.getElementById("infoHr").style.animation = 'none'
        document.getElementById("tableUsers").style.animation = 'none'

        
        
    } else if (isClicked == 'showComments') {
        document.getElementById(userInfoFormId).style.display = 'none';
        document.getElementById(orderHistoryFormId).style.display = 'none';
        document.getElementById(commentForm).style.display = 'inline-block';
    }
}


function theFunction(theBool) {
    if (theBool) {
        document.getElementById("infoForm").style.animation = '1s ease-out 0s 1 openForm';
    } if (!theBool) {
        document.getElementById("infoForm").style.animation = 'none';
    }
}



function makeStarBurlywood(star) {
    if (star == 'star1') {
        document.getElementById(star).style.color = 'burlywood';
        document.getElementById('star2').style.color = 'black';
        document.getElementById('star3').style.color = 'black';
        document.getElementById('star4').style.color = 'black';
        document.getElementById('star5').style.color = 'black';
        document.getElementById("starInput").value = '1';

    } if (star == 'star2') {
        document.getElementById('star1').style.color = 'burlywood';
        document.getElementById(star).style.color = 'burlywood';
        document.getElementById('star3').style.color = 'black';
        document.getElementById('star4').style.color = 'black';
        document.getElementById('star5').style.color = 'black';
        document.getElementById("starInput").value = '2';

    } if (star == 'star3') {
        document.getElementById('star1').style.color = 'burlywood';
        document.getElementById('star2').style.color = 'burlywood';
        document.getElementById(star).style.color = 'burlywood';
        document.getElementById('star4').style.color = 'black';
        document.getElementById('star5').style.color = 'black';
        document.getElementById("starInput").value = '3';

    } if (star == 'star4') {
        document.getElementById('star1').style.color = 'burlywood';
        document.getElementById('star2').style.color = 'burlywood';
        document.getElementById('star3').style.color = 'burlywood';
        document.getElementById(star).style.color = 'burlywood';
        document.getElementById('star5').style.color = 'black';
        document.getElementById("starInput").value = '4';

    } if (star == 'star5') {
        document.getElementById('star1').style.color = 'burlywood';
        document.getElementById('star2').style.color = 'burlywood';
        document.getElementById('star3').style.color = 'burlywood';
        document.getElementById('star4').style.color = 'burlywood';
        document.getElementById(star).style.color = 'burlywood';
        document.getElementById("starInput").value = '5';

    }
}

function makeStarTan(star) {
    if (star == 'star1') {
        document.getElementById(star).style.color = 'tan';
        document.getElementById('star2').style.color = 'black';
        document.getElementById('star3').style.color = 'black';
        document.getElementById('star4').style.color = 'black';
        document.getElementById('star5').style.color = 'black';
    } if (star == 'star2') {
        document.getElementById('star1').style.color = 'tan';
        document.getElementById(star).style.color = 'tan';
        document.getElementById('star3').style.color = 'black';
        document.getElementById('star4').style.color = 'black';
        document.getElementById('star5').style.color = 'black';
    } if (star == 'star3') {
        document.getElementById('star1').style.color = 'tan';
        document.getElementById('star2').style.color = 'tan';
        document.getElementById(star).style.color = 'tan';
        document.getElementById('star4').style.color = 'black';
        document.getElementById('star5').style.color = 'black';
    } if (star == 'star4') {
        document.getElementById('star1').style.color = 'tan';
        document.getElementById('star2').style.color = 'tan';
        document.getElementById('star3').style.color = 'tan';
        document.getElementById(star).style.color = 'tan';
        document.getElementById('star5').style.color = 'black';
    } if (star == 'star5') {
        document.getElementById('star1').style.color = 'tan';
        document.getElementById('star2').style.color = 'tan';
        document.getElementById('star3').style.color = 'tan';
        document.getElementById('star4').style.color = 'tan';
        document.getElementById(star).style.color = 'tan';
    }
}

function makeStarBlack() {
    if (document.getElementById('star1').style.color == 'tan' || document.getElementById('star2').style.color == 'tan' || document.getElementById('star3').style.color == 'tan' || document.getElementById('star4').style.color == 'tan' || document.getElementById('star5').style.color == 'tan') {
        document.getElementById('star1').style.color = 'black';
        document.getElementById('star2').style.color = 'black';
        document.getElementById('star3').style.color = 'black';
        document.getElementById('star4').style.color = 'black';
        document.getElementById('star5').style.color = 'black';
        document.getElementById("starInput").value = '0';
    }
}



function burgerMenuFunction() {
    var userId = "rightNavUser";
    var loginId = "rightNavLogin";
    var logoutId = "rightNavLogout";
    var contactId = "rightNavContact";
    var checkoutId = "rightNavCheckout";
    var menuId = "rightNavMenu";
    if (document.getElementById(menuId).style.display == "block") {
        document.getElementById(userId).style.display = "none";
        document.getElementById(logoutId).style.display = "none";
        document.getElementById(contactId).style.display = "none";
        document.getElementById(checkoutId).style.display = "none";
        document.getElementById(menuId).style.display = "none";
        document.getElementById(loginId).style.display = "none";
    } else {
        document.getElementById(userId).style.display = "block";
        document.getElementById(logoutId).style.display = "block";
        document.getElementById(contactId).style.display = "block";
        document.getElementById(checkoutId).style.display = "block";
        document.getElementById(menuId).style.display = "block";
        document.getElementById(loginId).style.display = "block";
    }
}

