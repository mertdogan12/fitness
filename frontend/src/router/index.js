import { createRouter, createWebHistory } from 'vue-router'
import KursKalender from '../components/KursKalender.vue'
import TrainerLogin from '../components/TrainerLogin.vue'
import TrainerDashboard from '../components/TrainerDashboard.vue'

const routes = [
  {
    path: '/',
    component: KursKalender
  },
  {
    path: '/trainer/login',
    component: TrainerLogin
  },
  {
    path: '/trainer/dashboard',
    component: TrainerDashboard,
    meta: { requiresAuth: true }  // geschützte Route
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Zugriffsschutz: ohne Login kein Dashboard
router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !sessionStorage.getItem('trainerAuth')) {
    next('/trainer/login')
  } else {
    next()
  }
})

export default router