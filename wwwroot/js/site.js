function downloadFile(filename, base64Data) {
    const link = document.createElement('a');
    link.href = 'data:text/csv;base64,' + base64Data;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// ============ CHARTS ============

let performanceChart, topScorersChart, scorersChart, reboundsChart, assistsChart, evalChart;
let winLossChart, ptsChart, lastFiveChart;

function renderPerformanceChart(labels, points, avgLine) {
    const ctx = document.getElementById('performanceChart');
    if (!ctx) return;
    if (performanceChart) performanceChart.destroy();
    performanceChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Punti',
                    data: points,
                    borderColor: '#ab3600',
                    backgroundColor: 'rgba(171,54,0,0.1)',
                    borderWidth: 3,
                    fill: true,
                    tension: 0.3,
                    pointBackgroundColor: '#ab3600',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 3,
                    pointRadius: 6,
                },
                {
                    label: 'Media',
                    data: avgLine,
                    borderColor: '#7fa000',
                    borderWidth: 2,
                    borderDash: [8, 4],
                    fill: false,
                    pointRadius: 0,
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    max: 25,
                    ticks: { stepSize: 5, font: { family: 'Space Mono', size: 10 } },
                    grid: { color: 'rgba(0,0,0,0.05)' }
                },
                x: {
                    ticks: { font: { family: 'Space Mono', size: 10 } },
                    grid: { display: false }
                }
            }
        }
    });
}

function renderTopScorersChart(names, data) {
    const ctx = document.getElementById('topScorersChart');
    if (!ctx) return;
    if (topScorersChart) topScorersChart.destroy();
    topScorersChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: names,
            datasets: [{
                data: data,
                backgroundColor: ['#ab3600', '#ff5f1f', '#585e6f', '#7fa000', '#c1c6d9'],
                borderRadius: 0,
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            indexAxis: 'y',
            plugins: { legend: { display: false } },
            scales: {
                x: { beginAtZero: true, grid: { color: 'rgba(0,0,0,0.05)' } },
                y: { ticks: { font: { family: 'Archivo Narrow', size: 13, weight: '700' } } }
            }
        }
    });
}

function renderScorersChart(names, data) {
    const ctx = document.getElementById('scorersChart');
    if (!ctx) return;
    if (scorersChart) scorersChart.destroy();
    scorersChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: names,
            datasets: [{ data: data, backgroundColor: '#ab3600', borderRadius: 0 }]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
                y: { beginAtZero: true, ticks: { stepSize: 5 } }
            }
        }
    });
}

function renderReboundsChart(names, data) {
    const ctx = document.getElementById('reboundsChart');
    if (!ctx) return;
    if (reboundsChart) reboundsChart.destroy();
    reboundsChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: names,
            datasets: [{ data: data, backgroundColor: '#585e6f', borderRadius: 0 }]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: { y: { beginAtZero: true } }
        }
    });
}

function renderAssistsChart(names, data) {
    const ctx = document.getElementById('assistsChart');
    if (!ctx) return;
    if (assistsChart) assistsChart.destroy();
    assistsChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: names,
            datasets: [{ data: data, backgroundColor: '#7fa000', borderRadius: 0 }]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: { y: { beginAtZero: true } }
        }
    });
}

function renderEvalChart(names, data) {
    const ctx = document.getElementById('evalChart');
    if (!ctx) return;
    if (evalChart) evalChart.destroy();
    evalChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: names,
            datasets: [{ data: data, backgroundColor: '#ff5f1f', borderRadius: 0 }]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: { y: { beginAtZero: true } }
        }
    });
}

function renderWinLossChart(wins, losses) {
    const ctx = document.getElementById('winLossChart');
    if (!ctx) return;
    if (winLossChart) winLossChart.destroy();
    winLossChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Vittorie', 'Sconfitte'],
            datasets: [{
                data: [wins, losses],
                backgroundColor: ['#ab3600', '#c1c6d9'],
                borderWidth: 0,
            }]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { position: 'bottom' } }
        }
    });
}

function renderPtsChart(ptsFor, ptsAgainst) {
    const ctx = document.getElementById('ptsChart');
    if (!ctx) return;
    if (ptsChart) ptsChart.destroy();
    ptsChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Punti Fatti', 'Punti Subiti'],
            datasets: [{
                data: [ptsFor, ptsAgainst],
                backgroundColor: ['#ab3600', '#585e6f'],
                borderRadius: 0,
            }]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: { y: { beginAtZero: true } }
        }
    });
}

function renderLastFiveChart(labels, ourPts, theirPts) {
    const ctx = document.getElementById('lastFiveChart');
    if (!ctx) return;
    if (lastFiveChart) lastFiveChart.destroy();
    lastFiveChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Nostri Punti',
                    data: ourPts,
                    borderColor: '#ab3600',
                    backgroundColor: 'rgba(171,54,0,0.1)',
                    borderWidth: 3,
                    fill: true,
                    tension: 0.3,
                    pointRadius: 5,
                },
                {
                    label: 'Avversario',
                    data: theirPts,
                    borderColor: '#585e6f',
                    borderWidth: 2,
                    borderDash: [6, 3],
                    fill: false,
                    pointRadius: 4,
                }
            ]
        },
        options: {
            responsive: true, maintainAspectRatio: false,
            plugins: { legend: { position: 'bottom', labels: { font: { family: 'Space Mono', size: 10 } } } },
            scales: {
                y: { beginAtZero: true, max: 25, ticks: { stepSize: 5 } }
            }
        }
    });
}
