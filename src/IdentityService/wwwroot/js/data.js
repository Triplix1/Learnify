const cardInput = document.getElementById('card');
const teacherFields = document.getElementById('teacherFields');
document.getElementById('type').addEventListener('change', function () {
    if (this.value === "0") {
        teacherFields.classList.add("hidden");
    } else {
        teacherFields.classList.remove("hidden");
    }
});

cardInput.onkeypress = verify;

function changedRole() {
    console.log("changed type")
    if (type.value === "Teacher") {
        teacherFields.classList.add("hidden");
    } else {
        teacherFields.classList.remove("hidden");
    }
}

function isKeyValid(key) {
    if (key > 47 && key < 58) return true;
    else if (key === 45) return true;
    else return false;
}

function isValidCard(arr, isDash) {
    const last = arr[arr.length - 1];
    if (last.length === 4 && !isDash) return false;
    else if (isDash && last.length !== 4) return false;
    else if (isDash && arr.length === 4) return false;
    else return true;
}

function verify(e) {
    const key = e.keyCode || e.which;
    const isDash = key === 45;
    const val = e.target.value;
    const input = val.split('-');
    if (!isKeyValid(key) || !isValidCard(input, isDash)) {
        return e.preventDefault();
    }
    // ...do something
}