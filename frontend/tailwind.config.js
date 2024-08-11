/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        'mv-purple': 'var(--MV-Purple)',
        'mv-light-purple': 'var(--MV-Purple)',
        'mv-purple-200': 'var(--MV-Purple)',
        'mv-violet': 'var(--MV-Violet)',
        'mv-slate': 'var(--MV-Slate)',
        'mv-slate-200': 'var(--MV-Slate-200)',
        'mv-gray': 'var(--MV-Gray)',
        'mv-red': 'var(--MV-Red)',
      },
    },
  },
  plugins: [],
}
